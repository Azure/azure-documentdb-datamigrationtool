using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Collections;
using Microsoft.DataTransfer.DocumentDb.Client;
using Microsoft.DataTransfer.DocumentDb.Shared;
using Microsoft.DataTransfer.DocumentDb.Transformation;
using Microsoft.DataTransfer.Extensibility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    sealed class DocumentDbBulkSinkAdapter : DocumentDbAdapterBase<IDocumentDbWriteClient, IDocumentDbBulkSinkAdapterInstanceConfiguration>, IDataSinkAdapter
    {
        private const string BulkImportStoredProcPrefix = "BulkImport";

        private string storedProcedureLink;
        private FastForwardBuffer<object> buffer;
        private LengthCappedEnumerableSurrogate surrogate;
        private int currentItemIndex;
        private ConcurrentDictionary<int, TaskCompletionSource<object>> activeBulkItems;

        private SemaphoreSlim flushSemaphore;

        public int MaxDegreeOfParallelism
        {
            get { return Configuration.BatchSize; }
        }

        public DocumentDbBulkSinkAdapter(IDocumentDbWriteClient client, IDataItemTransformation transformation, IDocumentDbBulkSinkAdapterInstanceConfiguration configuration)
            : base(client, transformation, configuration) { }

        public async Task InitializeAsync()
        {
            var collectionLink = await Client.GetOrCreateCollectionAsync(Configuration.CollectionName, Configuration.CollectionTier);
            storedProcedureLink = await Client.CreateStoredProcedureAsync(collectionLink,
                BulkImportStoredProcPrefix + Guid.NewGuid().ToString("N"), Configuration.StoredProcBody);

            buffer = new FastForwardBuffer<object>();
            surrogate = new LengthCappedEnumerableSurrogate(buffer, Configuration.MaxScriptSize);
            activeBulkItems = new ConcurrentDictionary<int, TaskCompletionSource<object>>();

            flushSemaphore = new SemaphoreSlim(1);
        }

        public Task WriteAsync(IDataItem dataItem, CancellationToken cancellation)
        {
            if (String.IsNullOrEmpty(storedProcedureLink))
                throw Errors.SinkIsNotInitialized();

            currentItemIndex = Interlocked.Increment(ref currentItemIndex);
            buffer.Add(new BulkItemSurrogate(currentItemIndex, Transformation.Transform(dataItem)));

            TaskCompletionSource<object> waitTaskCompletionSource;

            if (buffer.Count < Configuration.BatchSize)
            {
                if (!activeBulkItems.TryAdd(currentItemIndex, waitTaskCompletionSource = new TaskCompletionSource<object>()))
                    throw Errors.BufferSlotIsOccupied();

                return waitTaskCompletionSource.Task;
            }

            return FlushCurrentBufferSynchronizedAsync(currentItemIndex);
        }

        public async Task CompleteAsync(CancellationToken cancellation)
        {
            ExceptionDispatchInfo exceptionInfo = null;

            try
            {
                while (buffer.Count > 0)
                    await FlushCurrentBufferSynchronizedAsync(null);
            }
            catch (Exception exception)
            {
                exceptionInfo = ExceptionDispatchInfo.Capture(exception);
            }

            await CleanupAsync();

            if (exceptionInfo != null)
                exceptionInfo.Throw();
        }

        private async Task FlushCurrentBufferSynchronizedAsync(int? ownedDocumentIndex)
        {
            // Synchronized version to prevent multiple flushes happening in parallel
            // This is necessary when we got to the end, and CompleteAsync is about to flush
            if (buffer.Count <= 0)
                return;

            await flushSemaphore.WaitAsync();
            try
            {
                if (buffer.Count > 0)
                    await FlushCurrentBufferAsync(ownedDocumentIndex);
            }
            finally
            {
                flushSemaphore.Release();
            }
        }

        private async Task FlushCurrentBufferAsync(int? ownedDocumentIndex)
        {
            IEnumerable<BulkInsertItemState> response = null;
            try
            {
                response = await Client.ExecuteStoredProcedureAsync<IEnumerable<BulkInsertItemState>>(storedProcedureLink, surrogate, Configuration.DisableIdGeneration);
            }
            catch (Exception exception)
            {
                // In case of a stored procedure failure - fail all items: sorry, it's a bulk operation :)
                FailAllActiveBatches(exception, ownedDocumentIndex.HasValue);

                // If executing as part of the write - rethrow, otherwise ignore - will be reported on the item's task
                if (ownedDocumentIndex.HasValue) throw;

                return;
            }

            // Clean-up the buffer, because as soon as we start reporting completion - more items will be added, and buffer is not thread-safe
            buffer.SkipForward(response.Count());

            Exception ownedDocumentError = null;
            foreach (var item in response)
            {
                var itemError = String.IsNullOrEmpty(item.ErrorMessage) ? null : Errors.FailedToCreateDocument(item.ErrorMessage);

                TaskCompletionSource<object> waitHandle;
                if (!activeBulkItems.TryRemove(item.DocumentIndex, out waitHandle))
                {
                    // We are in the task that is responsible to report the state of the document, preserve the error
                    if (item.DocumentIndex == ownedDocumentIndex && itemError != null)
                        ownedDocumentError = itemError;
                    continue;
                }

                if (itemError == null)
                    waitHandle.SetResult(null);
                else
                    waitHandle.SetException(itemError);
            }

            if (ownedDocumentError != null)
                throw ownedDocumentError;
        }

        private void FailAllActiveBatches(Exception exception, bool skipSelf)
        {
            // Obtain all active items' indexes, so we can report exception on their tasks
            // ConcurrentDictionary<,>.Keys will actually clone the collection, so we are good
            var activeIndexes = activeBulkItems.Keys;

            // Clean-up the buffer first - it is not thread-safe
            // If we are running as part of the write task - our index will not be in activeIndexes - account for it based on the skipSelf
            buffer.SkipForward(activeIndexes.Count + (skipSelf ? 1 : 0));

            TaskCompletionSource<object> waitHandle;
            foreach (var index in activeIndexes)
                if (activeBulkItems.TryRemove(index, out waitHandle))
                    waitHandle.SetException(exception);
        }

        private async Task CleanupAsync()
        {
            if (!String.IsNullOrEmpty(storedProcedureLink))
                await Client.DeleteStoredProcedureAsync(storedProcedureLink);
        }

        public override void Dispose()
        {
            TrashCan.Throw(ref flushSemaphore);
            base.Dispose();
        }
    }
}
