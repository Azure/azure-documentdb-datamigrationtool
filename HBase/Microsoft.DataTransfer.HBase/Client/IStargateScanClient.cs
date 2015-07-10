using Microsoft.DataTransfer.HBase.Client.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.HBase.Client
{
    interface IStargateScanClient
    {
        Task<IReadOnlyList<HBaseRow>> ScanNextChunkAsync(IScannerReference scanner, CancellationToken cancellation);
        Task CloseScannerAsync(IScannerReference scanner, CancellationToken cancellation);
    }
}
