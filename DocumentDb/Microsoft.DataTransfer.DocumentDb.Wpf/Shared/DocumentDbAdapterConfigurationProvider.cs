using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Shared
{
    abstract class DocumentDbAdapterConfigurationProvider<TConfiguration, TShared> : DataAdapterValidatableConfigurationProviderBase<TConfiguration>
        where TConfiguration : DocumentDbAdapterConfiguration<TShared>
        where TShared : class, ISharedDocumentDbAdapterConfiguration 
    {
        protected override void PopulateCommandLineArguments(TConfiguration configuration, IDictionary<string, string> arguments)
        {
            Guard.NotNull("configuration", configuration);
            Guard.NotNull("arguments", arguments);

            arguments.Add(
                DocumentDbAdapterConfiguration<ISharedDocumentDbAdapterConfiguration>.ConnectionStringPropertyName,
                configuration.ConnectionString);

            if (configuration.ConnectionMode.HasValue && configuration.ConnectionMode.Value != Defaults.Current.ConnectionMode)
                arguments.Add(
                    DocumentDbAdapterConfiguration<ISharedDocumentDbAdapterConfiguration>.ConnectionModePropertyName,
                    configuration.ConnectionMode.Value.ToString());

            if (configuration.Retries.HasValue && configuration.Retries.Value != Defaults.Current.NumberOfRetries)
                arguments.Add(
                    DocumentDbAdapterConfiguration<ISharedDocumentDbAdapterConfiguration>.RetriesPropertyName,
                    configuration.Retries.Value.ToString(CultureInfo.InvariantCulture));

            if (configuration.RetryInterval.HasValue && configuration.RetryInterval.Value != Defaults.Current.RetryInterval)
                arguments.Add(
                    DocumentDbAdapterConfiguration<ISharedDocumentDbAdapterConfiguration>.RetryIntervalPropertyName,
                    configuration.RetryInterval.Value.ToString("c", CultureInfo.InvariantCulture));
        }
    }
}
