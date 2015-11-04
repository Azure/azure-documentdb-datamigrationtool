using Microsoft.DataTransfer.ServiceModel.Errors;
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

        public async Task<ITransferStatistics> Create(IErrorDetailsProvider errorDetailsProvider, ITransferStatisticsConfiguration configuration,
            CancellationToken cancellation)
        {
            var defaultStatistics = await defaultFactory.Create(errorDetailsProvider, configuration, cancellation);
            return String.IsNullOrEmpty(configuration.ErrorLog)
                ? new ObservableErrorsTransferStatistics(defaultStatistics, errorDetailsProvider, SynchronizationContext.Current)
                : defaultStatistics;
        }
    }
}
