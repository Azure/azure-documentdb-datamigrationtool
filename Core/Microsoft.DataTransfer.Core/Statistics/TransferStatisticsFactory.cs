using Microsoft.DataTransfer.ServiceModel;
using Microsoft.DataTransfer.ServiceModel.Entities;

namespace Microsoft.DataTransfer.Core.Statistics
{
    sealed class TransferStatisticsFactory : ITransferStatisticsFactory
    {
        public ITransferStatistics Create()
        {
            return new ThreadSafeTransferStatistics();
        }
    }
}
