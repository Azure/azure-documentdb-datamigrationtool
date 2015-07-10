using Microsoft.DataTransfer.ServiceModel.Statistics;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.WpfHost.Model.Statistics
{
    sealed class ObservableErrorsTransferStatisticsFactory : ITransferStatisticsFactory
    {
        private readonly ITransferStatisticsFactory defaultFactory;

        public ObservableErrorsTransferStatisticsFactory(ITransferStatisticsFactory defaultFactory)
        {
            this.defaultFactory = defaultFactory;
        }

        public async Task<ITransferStatistics> Create(ITransferStatisticsConfiguration configuration, CancellationToken cancellation)
        {
            var defaultStatistics = await defaultFactory.Create(configuration, cancellation);
            return String.IsNullOrEmpty(configuration.ErrorLog)
                ? new ObservableErrorsTransferStatistics(defaultStatistics, SynchronizationContext.Current)
                : defaultStatistics;
        }
    }
}
