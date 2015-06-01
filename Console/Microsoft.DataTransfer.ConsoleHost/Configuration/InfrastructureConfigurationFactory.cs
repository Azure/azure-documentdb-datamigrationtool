using Microsoft.DataTransfer.ConsoleHost.DynamicConfiguration;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.ConsoleHost.Configuration
{
    sealed class InfrastructureConfigurationFactory : IInfrastructureConfigurationFactory
    {
        private readonly DynamicConfigurationFactory configurationFactory;

        public InfrastructureConfigurationFactory()
        {
            configurationFactory = new DynamicConfigurationFactory();
        }

        public IInfrastructureConfiguration Create(IReadOnlyDictionary<string, string> arguments)
        {
            return (IInfrastructureConfiguration)configurationFactory.TryCreate(typeof(IInfrastructureConfiguration), arguments);
        }

        public IReadOnlyDictionary<string, string> DescribeOptions()
        {
            return configurationFactory.TryGetConfigurationOptions(typeof(IInfrastructureConfiguration));
        }
    }
}
