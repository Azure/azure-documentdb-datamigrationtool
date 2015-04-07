using Microsoft.DataTransfer.ServiceModel.Entities;

namespace Microsoft.DataTransfer.ConsoleHost.App.Handlers
{
    interface ITransferStatisticsHandler
    {
        ITransferStatistics CreateNew();
        void PrintProgress(ITransferStatisticsSnapshot statistics);
        void PrintResult(ITransferStatisticsSnapshot statistics);
    }
}
