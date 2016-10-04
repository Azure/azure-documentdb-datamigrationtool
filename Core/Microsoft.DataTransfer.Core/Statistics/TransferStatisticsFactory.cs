using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Files.Sink;
using Microsoft.DataTransfer.ServiceModel.Errors;
using Microsoft.DataTransfer.ServiceModel.Statistics;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Core.Statistics
{
    sealed class TransferStatisticsFactory : ITransferStatisticsFactory
    {
        public async Task<ITransferStatistics> Create(IErrorDetailsProvider errorDetailsProvider, ITransferStatisticsConfiguration configuration,
            CancellationToken cancellation)
        {
            Guard.NotNull("configuration", configuration);

            return String.IsNullOrEmpty(configuration.ErrorLog)
                ? (ITransferStatistics)new InMemoryTransferStatistics(errorDetailsProvider)
                : new CsvErrorLogTransferStatistics(
                    new StreamWriter(
                        await SinkStreamProvidersFactory.Create(configuration.ErrorLog, false, configuration.OverwriteErrorLog)
                            .CreateStream(cancellation)),
                    errorDetailsProvider);
        }
    }
}
