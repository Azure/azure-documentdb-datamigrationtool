using Microsoft.DataTransfer.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.TestsCommon
{
    public class DataTransferSinkTestBase : DataTransferTestBase
    {
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
