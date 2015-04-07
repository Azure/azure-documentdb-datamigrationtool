using Microsoft.DataTransfer.ServiceModel;
using Microsoft.DataTransfer.ServiceModel.Entities;
using Microsoft.DataTransfer.WpfHost.Extensibility;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DataTransfer.WpfHost.Model
{
    sealed class DataAdapterConfigurationProvidersCollection : IDataAdapterConfigurationProvidersCollection
    {
        private IReadOnlyCollection<KeyValuePair<string, IDataAdapterDefinition>> sources;
        private IReadOnlyCollection<KeyValuePair<string, IDataAdapterDefinition>> sinks;
        private IEnumerable<IDataAdapterConfigurationProvider> configurationProviders;

        public DataAdapterConfigurationProvidersCollection(IDataTransferService transferService, IEnumerable<IDataAdapterConfigurationProvider> configurationProviders)
        {
            sources = transferService.GetKnownSources();
            sinks = transferService.GetKnownSinks();
            this.configurationProviders = configurationProviders;
        }

        public IDataAdapterConfigurationProvider GetForSource(string source)
        {
            return GetByAdapterDefinition(sources.FirstOrDefault(s => s.Key == source).Value);
        }

        public IDataAdapterConfigurationProvider GetForSink(string sink)
        {
            return GetByAdapterDefinition(sinks.FirstOrDefault(s => s.Key == sink).Value);
        }

        private IDataAdapterConfigurationProvider GetByAdapterDefinition(IDataAdapterDefinition adapterDefinition)
        {
            return adapterDefinition == null ? null
                : configurationProviders.FirstOrDefault(p => p.CanProvide(adapterDefinition.ConfigurationType));
        }
    }
}
