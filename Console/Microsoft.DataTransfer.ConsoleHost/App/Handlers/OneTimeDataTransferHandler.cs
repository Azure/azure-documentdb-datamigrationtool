using Microsoft.DataTransfer.ConsoleHost.Configuration;
using Microsoft.DataTransfer.ConsoleHost.Extensibility;
using Microsoft.DataTransfer.ConsoleHost.Helpers;
using Microsoft.DataTransfer.ServiceModel;
using Microsoft.DataTransfer.ServiceModel.Entities;
using Microsoft.DataTransfer.ServiceModel.Statistics;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.ConsoleHost.App.Handlers
{
    sealed class OneTimeDataTransferHandler : ITransferHandler
    {
        private readonly IDataTransferService transferService;
        private readonly IDataAdapterConfigurationFactory dataAdapterConfiguration;
        private readonly IInfrastructureConfigurationFactory infrastructureConfiguration;
        private readonly ITransferStatisticsHandler statisticsHandler;

        private readonly IOneTimeDataTransferConfiguration configuration;

        public OneTimeDataTransferHandler(IDataTransferService transferService, IDataAdapterConfigurationFactory dataAdapterConfiguration,
            IInfrastructureConfigurationFactory infrastructureConfiguration, ITransferStatisticsHandler statisticsHandler, IOneTimeDataTransferConfiguration configuration)
        {
            this.transferService = transferService;
            this.dataAdapterConfiguration = dataAdapterConfiguration;
            this.infrastructureConfiguration = infrastructureConfiguration;
            this.statisticsHandler = statisticsHandler;
            this.configuration = configuration;
        }

        public async Task RunAsync()
        {
            ValidateConfiguration();

            IDataAdapterDefinition sourceDefinition;
            if (!transferService.GetKnownSources().TryGetValue(configuration.SourceName, out sourceDefinition))
                throw Errors.UnknownSource(configuration.SourceName);

            IDataAdapterDefinition sinkDefinition;
            if (!transferService.GetKnownSinks().TryGetValue(configuration.TargetName, out sinkDefinition))
                throw Errors.UnknownDestination(configuration.TargetName);

            var statistics = statisticsHandler.CreateNew(infrastructureConfiguration.Create(configuration.InfrastructureConfiguration));

            using (var cancellation = new ConsoleCancellationSource())
            {
                using (new Timer(PrintStatistics, statistics, TimeSpan.Zero, TimeSpan.FromSeconds(1)))
                {
                    await transferService
                        .TransferAsync(
                        // From
                            configuration.SourceName,
                            dataAdapterConfiguration.TryCreate(sourceDefinition.ConfigurationType, configuration.SourceConfiguration),
                        // To
                            configuration.TargetName,
                            dataAdapterConfiguration.TryCreate(sinkDefinition.ConfigurationType, configuration.TargetConfiguration),
                        // With statistics
                            statistics,
                        // Allow cancellation
                            cancellation.Token);
                }
            }

            statisticsHandler.PrintResult(statistics.GetSnapshot());
        }

        private void ValidateConfiguration()
        {
            if (String.IsNullOrEmpty(configuration.SourceName))
                throw Errors.SourceMissing();

            if (String.IsNullOrEmpty(configuration.TargetName))
                throw Errors.TargetMissing();
        }

        private void PrintStatistics(object state)
        {
            statisticsHandler.PrintProgress(((ITransferStatistics)state).GetSnapshot());
        }
    }
}
