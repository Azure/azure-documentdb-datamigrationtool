using Microsoft.DataTransfer.ConsoleHost.Configuration;
using Microsoft.DataTransfer.ServiceModel.Statistics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.ConsoleHost.App.Handlers
{
    interface ITransferStatisticsHandler
    {
        Task<ITransferStatistics> CreateNew(ITransferStatisticsConfiguration configuration, CancellationToken cancellation);
        void PrintProgress(ITransferStatisticsSnapshot statistics);
        void PrintResult(ITransferStatisticsSnapshot statistics);
        Task<ITransferStatistics> CreateNew(ITransferStatisticsConfiguration configuration, CancellationToken cancellation, IReadOnlyDictionary<string, string> destConfiguration);
    }
}
