using Microsoft.DataTransfer.Extensibility.Basics.Collections;
using Microsoft.DataTransfer.HBase.Client.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.HBase.Client
{
    interface IStargateClient
    {
        Task<string> GetClusterVersionAsync(CancellationToken cancellation);
        Task<IAsyncEnumerator<HBaseRow>> ScanAsync(string tableName, string filter, int batchSize, CancellationToken cancellation);
    }
}
