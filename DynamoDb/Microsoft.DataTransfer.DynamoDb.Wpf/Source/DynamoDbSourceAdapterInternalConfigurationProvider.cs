using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.DynamoDb.Wpf.Source
{
    sealed class DynamoDbSourceAdapterInternalConfigurationProvider : DataAdapterValidatableConfigurationProviderBase<DynamoDbSourceAdapterConfiguration>
    {
        protected override UserControl CreatePresenter(DynamoDbSourceAdapterConfiguration configuration)
        {
            return new DynamoDbSourceAdapterConfigurationPage() { DataContext = configuration };
        }

        protected override UserControl CreateSummaryPresenter(DynamoDbSourceAdapterConfiguration configuration)
        {
            return new DynamoDbSourceAdapterConfigurationSummaryPage { DataContext = configuration };
        }

        protected override DynamoDbSourceAdapterConfiguration CreateValidatableConfiguration()
        {
            return new DynamoDbSourceAdapterConfiguration();
        }

        protected override void PopulateCommandLineArguments(DynamoDbSourceAdapterConfiguration configuration, IDictionary<string, string> arguments)
        {
            Guard.NotNull("configuration", configuration);
            Guard.NotNull("arguments", arguments);

            arguments.Add(DynamoDbSourceAdapterConfiguration.ConnectionStringPropertyName, configuration.ConnectionString);

            if (configuration.UseRequestFile)
            {
                if (!String.IsNullOrEmpty(configuration.RequestFile))
                    arguments.Add(DynamoDbSourceAdapterConfiguration.RequestFilePropertyName, configuration.RequestFile);
            }
            else
            {
                if (!String.IsNullOrEmpty(configuration.Request))
                    arguments.Add(DynamoDbSourceAdapterConfiguration.RequestPropertyName, configuration.Request);
            }
        }
    }
}
