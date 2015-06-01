using Microsoft.DataTransfer.ServiceModel.Statistics;

namespace Microsoft.DataTransfer.ConsoleHost.App.Handlers
{
    interface ITransferStatisticsHandler
    {
        ITransferStatistics CreateNew(ITransferStatisticsConfiguration configuration);
        void PrintProgress(ITransferStatisticsSnapshot statistics);
        void PrintResult(ITransferStatisticsSnapshot statistics);
    }
}
