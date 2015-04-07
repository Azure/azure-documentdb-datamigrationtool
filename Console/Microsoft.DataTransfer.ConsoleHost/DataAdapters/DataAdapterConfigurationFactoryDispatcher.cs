using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.ConsoleHost.Extensibility;
using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.ConsoleHost.DataAdapters
{
    sealed class DataAdapterConfigurationFactoryDispatcher : IDataAdapterConfigurationFactory
    {
        private IEnumerable<IDataAdapterConfigurationFactory> factories;

        public DataAdapterConfigurationFactoryDispatcher(IEnumerable<IDataAdapterConfigurationFactory> factories)
        {
            this.factories = factories;
        }

        public object TryCreate(Type configurationType, IReadOnlyDictionary<string, string> arguments)
        {
            Guard.NotNull("configurationType", configurationType);

            foreach (var factory in factories)
            {
                var configuration = factory.TryCreate(configurationType, arguments);
                if (configuration != null && configurationType.IsAssignableFrom(configuration.GetType()))
                    return configuration;
            }
            throw Errors.DataAdapterConfigurationFactoryNotFound(configurationType);
        }

        public IReadOnlyDictionary<string, string> TryGetConfigurationOptions(Type configurationType)
        {
            foreach (var factory in factories)
            {
                var options = factory.TryGetConfigurationOptions(configurationType);
                if (options != null)
                    return options;
            }
            throw Errors.DataAdapterConfigurationFactoryNotFound(configurationType);
        }
    }
}
