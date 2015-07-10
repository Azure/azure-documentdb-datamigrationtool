using Microsoft.DataTransfer.ServiceModel.Statistics;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.ConsoleHost.App.Handlers
{
    interface ITransferStatisticsHandler
    {
        Task<ITransferStatistics> CreateNew(ITransferStatisticsConfiguration configuration, CancellationToken cancellation);
        void PrintProgress(ITransferStatisticsSnapshot statistics);
        void PrintResult(ITransferStatisticsSnapshot statistics);
    }
}
