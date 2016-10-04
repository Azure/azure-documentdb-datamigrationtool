using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.AzureTable.Wpf.Shared
{
    abstract class AzureTableAdapterConfigurationProvider<TConfiguration> : DataAdapterValidatableConfigurationProviderBase<TConfiguration>
        where TConfiguration : AzureTableAdapterConfiguration
    {
        protected override void PopulateCommandLineArguments(TConfiguration configuration, IDictionary<string, string> arguments)
        {
            Guard.NotNull("configuration", configuration);
            Guard.NotNull("arguments", arguments);

            arguments.Add(AzureTableAdapterConfiguration.ConnectionStringPropertyName, configuration.ConnectionString);

            if (configuration.LocationMode.HasValue && configuration.LocationMode.Value != Defaults.Current.LocationMode)
                arguments.Add(
                    AzureTableAdapterConfiguration.LocationModePropertyName,
                    configuration.LocationMode.Value.ToString());
        }
    }
}
