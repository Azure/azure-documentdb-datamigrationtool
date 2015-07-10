using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Core.FactoryAdapters;
using Microsoft.DataTransfer.ServiceModel;
using Microsoft.DataTransfer.ServiceModel.Entities;
using Microsoft.DataTransfer.ServiceModel.Statistics;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Core.Service
{
    sealed class DataTransferService : IDataTransferService
    {
        private IDataTransferAction transferAction;

        private IReadOnlyDictionary<string, IDataSourceAdapterFactoryAdapter> sources;
        private IReadOnlyDictionary<string, IDataSinkAdapterFactoryAdapter> sinks;

        public DataTransferService(
            IReadOnlyDictionary<string, IDataSourceAdapterFactoryAdapter> sources,
            IReadOnlyDictionary<string, IDataSinkAdapterFactoryAdapter> sinks,
            IDataTransferAction transferAction)
        {
            Guard.NotNull("sources", sources);
            Guard.NotNull("sinks", sinks);
            Guard.NotNull("transferAction", transferAction);

            this.sources = sources;
            this.sinks = sinks;
            this.transferAction = transferAction;
        }

        public IReadOnlyDictionary<string, IDataAdapterDefinition> GetKnownSources()
        {
            return sources.ToDictionary(s => s.Key, s => (IDataAdapterDefinition)s.Value);
        }

        public IReadOnlyDictionary<string, IDataAdapterDefinition> GetKnownSinks()
        {
            return sinks.ToDictionary(s => s.Key, s => (IDataAdapterDefinition)s.Value);
        }

        public async Task TransferAsync(string sourceName, object sourceConfiguration,
            string sinkName, object sinkConfiguration, ITransferStatistics statistics, CancellationToken cancellation)
        {
            IDataSourceAdapterFactoryAdapter sourceFactoryAdapter;
            if (!sources.TryGetValue(sourceName, out sourceFactoryAdapter))
                throw Errors.UnknownDataSource(sourceName);

            IDataSinkAdapterFactoryAdapter sinkFactoryAdapter;
            if (!sinks.TryGetValue(sinkName, out sinkFactoryAdapter))
                throw Errors.UnknownDataSink(sinkName);

            var context = new DataTransferContext
            {
                SourceName = sourceName,
                SinkName = sinkName
            };

            try
            {
                // Lets start timer now, since factories may take some time as well and we want to capture that
                statistics.Start();

                var source = sourceFactoryAdapter.CreateAsync(sourceConfiguration, context, cancellation);
                var sink = sinkFactoryAdapter.CreateAsync(sinkConfiguration, context, cancellation);

                using (var sourceInstance = await source)
                using (var sinkInstance = await sink)
                    await transferAction.ExecuteAsync(sourceInstance, sinkInstance, statistics, cancellation);
            }
            finally
            {
                statistics.Stop();
            }
        }
    }
}
