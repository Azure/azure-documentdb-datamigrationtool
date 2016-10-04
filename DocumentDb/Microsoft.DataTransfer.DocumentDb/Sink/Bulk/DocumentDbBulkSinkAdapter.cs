using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Collections;
using Microsoft.DataTransfer.DocumentDb.Client;
using Microsoft.DataTransfer.DocumentDb.Exceptions;
using Microsoft.DataTransfer.DocumentDb.Shared;
using Microsoft.DataTransfer.DocumentDb.Transformation;
using Microsoft.DataTransfer.Extensibility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    sealed class DocumentDbBulkSinkAdapter : DocumentDbAdapterBase<IDocumentDbWriteClient, IDocumentDbBulkSinkAdapterInstanceConfiguration>, IDataSinkAdapter
    {
        private string storedProcedureLink;
        private FastForwardBuffer<BulkItemSurrogate> buffer;
        private LengthCappedEnumerableSurrogate surrogate;
        private int numberOfItems;
        private ConcurrentDictionary<int, TaskCompletionSource<object>> activeBulkItems;

        private SemaphoreSlim flushSemaphore;

        public int MaxDegreeOfParallelism
        {
            // Allow framework to read one bulk ahead, so that flush will not starve
            get { return Configuration.BatchSize * 2; }
        }

        public DocumentDbBulkSinkAdapter(IDocumentDbWriteClient client, IDataItemTransformation transformation, IDocumentDbBulkSinkAdapterInstanceConfiguration configuration)
            : base(client, transformation, configuration) { }

        public async Task InitializeAsync(CancellationToken cancellation)
        {
            var collectionLink = await Client.GetOrCreateCollectionAsync(
                Configuration.Collection, null, Configuration.CollectionThroughput, Configuration.IndexingPolicy, cancellation);

            storedProcedureLink = await Client.CreateStoredProcedureAsync(
                collectionLink, Configuration.StoredProcName, Configuration.StoredProcBody);

            buffer = new FastForwardBuffer<BulkItemSurrogate>();
            surrogate = new LengthCappedEnumerableSurrogate(buffer, Configuration.BatchSize, Configuration.MaxScriptSize);
            activeBulkItems = new ConcurrentDictionary<int, TaskCompletionSource<object>>();

            flushSemaphore = new SemaphoreSlim(1, 1);
        }

        public Task WriteAsync(IDataItem dataItem, CancellationToken cancellation)
        {
            if (String.IsNullOrEmpty(storedProcedureLink))
                throw Errors.SinkIsNotInitialized();

            var currentItemIndex = Interlocked.Increment(ref numberOfItems);
            buffer.Add(new BulkItemSurrogate(currentItemIndex, Transformation.Transform(dataItem)));

            var waitTaskCompletionSource = new TaskCompletionSource<object>();
            if (!activeBulkItems.TryAdd(currentItemIndex, waitTaskCompletionSource))
                throw Errors.BufferSlotIsOccupied();

            if (HaveEnoughRecordsForBatch() && flushSemaphore.Wait(0))
            {
                // If we are not flushing yet - fire and forget flush task
                FlushWhileAsync(HaveEnoughRecordsForBatch, true, cancellation)
                    .ContinueWith(HandleUnexpectedFlushFailure, TaskContinuationOptions.OnlyOnFaulted);
            }

            return waitTaskCompletionSource.Task;
        }

        private void HandleUnexpectedFlushFailure(Task failed)
        {
            // When async flush operation fails - report it on the first document in the list.
            // It is not related to the document, but we need to report it somewhere, so that
            // it will not go unnoticed.
            foreach (var index in activeBulkItems.Keys)
            {
                TaskCompletionSource<object> waitHandle;
                if (!activeBulkItems.TryRemove(index, out waitHandle))
                    continue;

                waitHandle.SetException(Errors.UnexpectedAsyncFlushError(failed.Exception));
            }

            // It might be so that we cannot find anything - let's assume we flushed everything successfully
        }

        public async Task CompleteAsync(CancellationToken cancellation)
        {
            ExceptionDispatchInfo exceptionInfo = null;

            try
            {
                await FlushWhileAsync(HaveAnyRecords, false, CancellationToken.None);
            }
            catch (Exception exception)
            {
                exceptionInfo = ExceptionDispatchInfo.Capture(exception);
            }

            await CleanupAsync();

            if (exceptionInfo != null)
                exceptionInfo.Throw();
        }

        private async Task FlushWhileAsync(Func<bool> condition, bool lockAcquired, CancellationToken cancellationToken)
        {
            // NOTE: This task will run in the background, so we need to make sure it is robust enough

            try
            {
                while (condition())
                {
                    if (lockAcquired)
                    {
                        // Actual lock will be released in the finally
                        lockAcquired = false;
                    }
                    else
                    {
                        await flushSemaphore.WaitAsync(cancellationToken);
                    }

                    IEnumerable<BulkInsertItemState> response;
                    try
                    {
                        if (!condition())
                            continue;

                        response = await FlushCurrentBufferAsync();
                        buffer.SkipForward(response.Count());
                    }
                    finally
                    {
                        flushSemaphore.Release();
                    }

                    ReportTasksStatus(response);
                }
            }
            finally
            {
                if (lockAcquired)
                    // If while condition was not satisfied - finally block did not execute, release the lock
                    flushSemaphore.Release();
            }
        }

        private async Task<IEnumerable<BulkInsertItemState>> FlushCurrentBufferAsync()
        {
            IEnumerable<BulkInsertItemState> result = null;
            try
            {
                var response = await Client.ExecuteStoredProcedureAsync<IEnumerable<BulkInsertItemState>>(
                    storedProcedureLink, surrogate, Configuration.UpdateExisting ? 1 : 0, Configuration.DisableIdGeneration ? 1 : 0);

                result = response.Data;

                // Append global ActivityId to all error messages
                foreach (var item in result)
                {
                    if (!String.IsNullOrEmpty(item.ErrorMessage))
                        item.ErrorMessage += String.Format(CultureInfo.InvariantCulture,
                            Resources.ImportErrorActivityIdFormat, Environment.NewLine, response.ActivityId);
                }
            }
            catch (DocumentSizeExceedsScriptSizeLimitException documentTooLarge)
            {
                // This error is thrown by the capped serializer if very first document got capped by the
                // size limit and prevents it from sending an empty request.
                // In this case - remove the first document from the buffer and report an error.
                var firstDocument = buffer.FirstOrDefault();
                if (firstDocument != null)
                {
                    result = new BulkInsertItemState[] 
                    {
                        new BulkInsertItemState
                        {
                            DocumentIndex = firstDocument.DocumentIndex,
                            ErrorMessage = documentTooLarge.Message
                        }
                    };
                }
            }
            catch (Exception exception)
            {
                // Obtain last serialized document indexes, and report exception on their tasks
                // ConcurrentDictionary<,>.Keys will actually clone the collection, so we are good
                var lastIndexes = activeBulkItems.Keys;

                var responseIndex = -1;
                var responseArray = new BulkInsertItemState[surrogate.LastSerializedCount];
                foreach (var index in lastIndexes)
                {
                    if (++responseIndex >= surrogate.LastSerializedCount)
                        break;

                    responseArray[responseIndex] = new BulkInsertItemState
                    {
                        DocumentIndex = index,
                        ErrorMessage = exception.Message
                    };
                }

                result = responseArray;
            }

            return result ?? Enumerable.Empty<BulkInsertItemState>();
        }

        private void ReportTasksStatus(IEnumerable<BulkInsertItemState> response)
        {
            foreach (var item in response)
            {
                var itemError = String.IsNullOrEmpty(item.ErrorMessage) ? null : Errors.FailedToCreateDocument(item.ErrorMessage);

                TaskCompletionSource<object> waitHandle;
                if (!activeBulkItems.TryRemove(item.DocumentIndex, out waitHandle))
                    continue;

                if (itemError == null)
                    waitHandle.SetResult(null);
                else
                    waitHandle.SetException(itemError);
            }
        }

        private async Task CleanupAsync()
        {
            if (!String.IsNullOrEmpty(storedProcedureLink))
                await Client.DeleteStoredProcedureAsync(storedProcedureLink);
        }

        private bool HaveAnyRecords()
        {
            return buffer.Count > 0;
        }

        private bool HaveEnoughRecordsForBatch()
        {
            return buffer.Count > Configuration.BatchSize;
        }

        public override void Dispose()
        {
            TrashCan.Throw(ref flushSemaphore);
            base.Dispose();
        }
    }
}
