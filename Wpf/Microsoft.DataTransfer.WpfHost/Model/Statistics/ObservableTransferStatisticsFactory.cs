using Microsoft.DataTransfer.ServiceModel;
using Microsoft.DataTransfer.ServiceModel.Entities;
using System.Threading;

namespace Microsoft.DataTransfer.WpfHost.Model.Statistics
{
    sealed class ObservableTransferStatisticsFactory : ITransferStatisticsFactory
    {
        private readonly ITransferStatisticsFactory defaultFactory;

        public ObservableTransferStatisticsFactory(ITransferStatisticsFactory defaultFactory)
        {
            this.defaultFactory = defaultFactory;
        }

        public ITransferStatistics Create()
        {
            return new ObservableTransferStatistics(defaultFactory.Create(), SynchronizationContext.Current);
        }
    }
}
