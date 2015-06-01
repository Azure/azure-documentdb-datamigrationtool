using Microsoft.DataTransfer.ServiceModel.Statistics;
using System;
using System.IO;

namespace Microsoft.DataTransfer.Core.Statistics
{
    sealed class TransferStatisticsFactory : ITransferStatisticsFactory
    {
        public ITransferStatistics Create(ITransferStatisticsConfiguration configuration)
        {
            return String.IsNullOrEmpty(configuration.ErrorLog)
                ? (ITransferStatistics)new InMemoryTransferStatistics()
                : new CsvErrorLogTransferStatistics(CreateErrorLog(configuration));
        }

        private static FileStream CreateErrorLog(ITransferStatisticsConfiguration configuration)
        {
            // Ensure output folder exists
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(configuration.ErrorLog));
            }
            catch { }

            return File.Open(configuration.ErrorLog, configuration.OverwriteErrorLog ? FileMode.Create : FileMode.CreateNew);
        }
    }
}
