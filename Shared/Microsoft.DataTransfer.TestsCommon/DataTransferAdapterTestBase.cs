using Microsoft.DataTransfer.Extensibility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.TestsCommon
{
    public class DataTransferAdapterTestBase : DataTransferTestBase
    {
        protected async Task<List<IDataItem>> ReadDataAsync(IDataSourceAdapter sourceAdapter)
        {
            var results = new List<IDataItem>();

            IDataItem dataItem;
            var readOutput = new ReadOutputByRef();

            while (true)
            {
                try
                {
                    dataItem = await sourceAdapter.ReadNextAsync(readOutput, CancellationToken.None);
                }
                catch (NonFatalReadException)
                {
                    continue;
                }

                if (dataItem == null)
                    break;

                results.Add(dataItem);

                Assert.IsNotNull(readOutput.DataItemId, CommonTestResources.MissingDataItemId);
                readOutput.Wipe();
            }

            return results;
        }

        protected async Task WriteDataAsync(IDataSinkAdapter sinkAdapter, IEnumerable<IDataItem> data)
        {
            Task[] writeTasks = Enumerable
                .Range(0, sinkAdapter.MaxDegreeOfParallelism)
                .Select(i => (Task)Task.FromResult<object>(null))
                .ToArray();

            foreach (var dataItem in data)
            {
                var completed = await Task.WhenAny(writeTasks);
                await completed;
                writeTasks[Array.IndexOf(writeTasks, completed)] =
                    sinkAdapter.WriteAsync(dataItem, CancellationToken.None);
            }

            await sinkAdapter.CompleteAsync(CancellationToken.None);

            for (var index = 0; index < writeTasks.Length; ++index)
                await writeTasks[index];
        }
    }
}
