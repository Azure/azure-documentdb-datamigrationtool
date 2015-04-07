using Microsoft.DataTransfer.ServiceModel;
using Microsoft.DataTransfer.ServiceModel.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Microsoft.DataTransfer.ConsoleHost.App.Handlers
{
    sealed class TransferStatisticsHandler : ITransferStatisticsHandler
    {
        private ITransferStatisticsFactory statisticsFactory;

        public TransferStatisticsHandler(ITransferStatisticsFactory statisticsFactory)
        {
            this.statisticsFactory = statisticsFactory;
        }

        public ITransferStatistics CreateNew()
        {
            return statisticsFactory.Create();
        }

        public void PrintProgress(ITransferStatisticsSnapshot statistics)
        {
            var message = String.Format(CultureInfo.InvariantCulture, Resources.StatisticsProgressFormat,
                statistics.Transferred, statistics.GetErrors().Count, statistics.ElapsedTime);

            Console.Write("\r{0}", message);
        }

        public void PrintResult(ITransferStatisticsSnapshot statistics)
        {
            Console.Write("\r{0}\r", new String(' ', Console.WindowWidth - 1));

            var errors = statistics.GetErrors();
            Console.WriteLine(String.Format(CultureInfo.InvariantCulture, Resources.StatisticsResultFormat,
                statistics.Transferred, errors.Count, statistics.ElapsedTime));

            if (errors.Count > 0)
                PrintFailures(errors);
        }

        private static void PrintFailures(IReadOnlyCollection<KeyValuePair<string, Exception>> errors)
        {
            Console.WriteLine();
            Console.WriteLine(Resources.StatisticsFailuresHeader);
            Console.WriteLine(String.Join(Environment.NewLine, 
                errors.Select(e => String.Format(CultureInfo.InvariantCulture, 
                    Resources.StatisticsFailureItemFormat, e.Key, e.Value.Message))));
        }
    }
}
