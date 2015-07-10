using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.ServiceModel;
using Microsoft.DataTransfer.ServiceModel.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Core.Service
{
    sealed class DataTransferAction : IDataTransferAction
    {
        public async Task ExecuteAsync(IDataSourceAdapter source, IDataSinkAdapter sink, ITransferStatistics statistics, CancellationToken cancellation)
        {
            Guard.NotNull("source", source);
            Guard.NotNull("sink", sink);
            Guard.NotNull("statistics", statistics);

            var writeTasks = Enumerable
                    .Range(0, sink.MaxDegreeOfParallelism)
                    .Select(i => (Task)Task.FromResult<object>(null))
                    .ToArray();

            var fatalExceptions = new List<Exception>();
            var readOutput = new ReadOutputByRef();
            IDataItem dataItem;
            while (!cancellation.IsCancellationRequested)
            {
                readOutput.Wipe();

                try
                {
                    dataItem = await source.ReadNextAsync(readOutput, cancellation);
                }
                catch (NonFatalReadException nonFatalException)
                {
                    statistics.AddError(readOutput.DataItemId, nonFatalException);
                    continue;
                }
                catch (Exception exception)
                {
                    fatalExceptions.Add(exception);
                    break;
                }

                if (dataItem == null || cancellation.IsCancellationRequested)
                    break;

                var completed = await Task.WhenAny(writeTasks);
                writeTasks[Array.IndexOf(writeTasks, completed)] =
                    TransferDataItem(sink, readOutput.DataItemId, dataItem, statistics, cancellation);
            }

            // Report completion to the sink
            try
            {
                await sink.CompleteAsync(cancellation);
            }
            catch (Exception exception)
            {
                fatalExceptions.Add(exception);
            }

            // Wait for all on-going writes to complete
            for (var index = 0; index < writeTasks.Length; ++index)
                await writeTasks[index];

            // Throw fatal exceptions, if any
            if (fatalExceptions.Any())
                throw new AggregateException(fatalExceptions);
        }

        private static async Task TransferDataItem(IDataSinkAdapter sink, string dataItemId, IDataItem dataItem, ITransferStatistics statistics, CancellationToken cancellation)
        {
            try
            {
                await sink.WriteAsync(dataItem, cancellation);
                statistics.AddTransferred();
            }
            catch (AggregateException aggregateException)
            {
                foreach (var exception in aggregateException.Flatten().InnerExceptions)
                    statistics.AddError(dataItemId, exception);
            }
            catch (Exception exception)
            {
                statistics.AddError(dataItemId, exception);
            }
        }
    }
}
