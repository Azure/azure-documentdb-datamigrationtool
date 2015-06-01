using Microsoft.DataTransfer.ServiceModel.Statistics;
using System;
using System.Threading;

namespace Microsoft.DataTransfer.WpfHost.Model.Statistics
{
    sealed class ObservableErrorsTransferStatisticsFactory : ITransferStatisticsFactory
    {
        private readonly ITransferStatisticsFactory defaultFactory;

        public ObservableErrorsTransferStatisticsFactory(ITransferStatisticsFactory defaultFactory)
        {
            this.defaultFactory = defaultFactory;
        }

        public ITransferStatistics Create(ITransferStatisticsConfiguration configuration)
        {
            var defaultStatistics = defaultFactory.Create(configuration);
            return String.IsNullOrEmpty(configuration.ErrorLog)
                ? new ObservableErrorsTransferStatistics(defaultStatistics, SynchronizationContext.Current)
                : defaultStatistics;
        }
    }
}
