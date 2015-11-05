using Microsoft.DataTransfer.ServiceModel.Errors;
using Microsoft.DataTransfer.ServiceModel.Statistics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.ConsoleHost.App.Handlers
{
    sealed class TransferStatisticsHandler : ITransferStatisticsHandler
    {
        private readonly ITransferStatisticsFactory statisticsFactory;
        private readonly IErrorDetailsProvider errorDetailsProvider;

        public TransferStatisticsHandler(ITransferStatisticsFactory statisticsFactory, IErrorDetailsProvider errorDetailsProvider)
        {
            this.statisticsFactory = statisticsFactory;
            this.errorDetailsProvider = errorDetailsProvider;
        }

        public Task<ITransferStatistics> CreateNew(ITransferStatisticsConfiguration configuration, CancellationToken cancellation)
        {
            return statisticsFactory.Create(errorDetailsProvider, configuration, cancellation);
        }

        public void PrintProgress(ITransferStatisticsSnapshot statistics)
        {
            var message = String.Format(CultureInfo.InvariantCulture, Resources.StatisticsProgressFormat,
                statistics.Transferred, statistics.Failed, statistics.ElapsedTime);

            Console.Write("\r{0}", message);
        }

        public void PrintResult(ITransferStatisticsSnapshot statistics)
        {
            if (Console.IsOutputRedirected)
                Console.WriteLine();
            else
                Console.Write("\r{0}\r", new String(' ', Console.WindowWidth - 1));

            Console.WriteLine(String.Format(CultureInfo.InvariantCulture, Resources.StatisticsResultFormat,
                statistics.Transferred, statistics.Failed, statistics.ElapsedTime));

            PrintFailures(statistics.GetErrors());
        }

        private static void PrintFailures(IReadOnlyCollection<KeyValuePair<string, string>> errors)
        {
            if (errors == null || errors.Count <= 0)
                return;

            Console.WriteLine();
            Console.WriteLine(Resources.StatisticsFailuresHeader);
            Console.WriteLine(String.Join(Environment.NewLine, 
                errors.Select(e => String.Format(CultureInfo.InvariantCulture,
                    Resources.StatisticsFailureItemFormat, e.Key, e.Value))));
        }
    }
}
