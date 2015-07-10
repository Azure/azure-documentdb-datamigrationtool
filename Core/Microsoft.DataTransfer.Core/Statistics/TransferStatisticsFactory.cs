using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Files.Sink;
using Microsoft.DataTransfer.ServiceModel.Statistics;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Core.Statistics
{
    sealed class TransferStatisticsFactory : ITransferStatisticsFactory
    {
        public async Task<ITransferStatistics> Create(ITransferStatisticsConfiguration configuration, CancellationToken cancellation)
        {
            Guard.NotNull("configuration", configuration);

            return String.IsNullOrEmpty(configuration.ErrorLog)
                ? (ITransferStatistics)new InMemoryTransferStatistics()
                : new CsvErrorLogTransferStatistics(await SinkStreamProvidersFactory.Create(
                    configuration.ErrorLog, configuration.OverwriteErrorLog).CreateWriter(cancellation));
        }
    }
}
