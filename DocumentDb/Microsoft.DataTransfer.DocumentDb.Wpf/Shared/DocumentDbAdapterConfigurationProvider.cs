using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Shared
{
    abstract class DocumentDbAdapterConfigurationProvider<TConfiguration> : DataAdapterValidatableConfigurationProviderBase<TConfiguration>
        where TConfiguration : DocumentDbAdapterConfiguration
    {
        protected override void PopulateCommandLineArguments(TConfiguration configuration, IDictionary<string, string> arguments)
        {
            Guard.NotNull("configuration", configuration);
            Guard.NotNull("arguments", arguments);

            arguments.Add(DocumentDbAdapterConfiguration.ConnectionStringPropertyName, configuration.ConnectionString);

            if (configuration.ConnectionMode.HasValue && configuration.ConnectionMode.Value != Defaults.Current.ConnectionMode)
                arguments.Add(
                    DocumentDbAdapterConfiguration.ConnectionModePropertyName,
                    configuration.ConnectionMode.Value.ToString());

            if (configuration.Retries.HasValue && configuration.Retries.Value != Defaults.Current.NumberOfRetries)
                arguments.Add(
                    DocumentDbAdapterConfiguration.RetriesPropertyName,
                    configuration.Retries.Value.ToString(CultureInfo.InvariantCulture));

            if (configuration.RetryInterval.HasValue && configuration.RetryInterval.Value != Defaults.Current.RetryInterval)
                arguments.Add(
                    DocumentDbAdapterConfiguration.RetryIntervalPropertyName,
                    configuration.RetryInterval.Value.ToString());
        }
    }
}
