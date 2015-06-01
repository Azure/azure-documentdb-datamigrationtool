using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.RavenDb.Wpf.Source
{
    sealed class RavenDbSourceAdapterInternalConfigurationProvider : DataAdapterValidatableConfigurationProviderBase<RavenDbSourceAdapterConfiguration>
    {
        protected override UserControl CreatePresenter(RavenDbSourceAdapterConfiguration configuration)
        {
            return new RavenDbSourceAdapterConfigurationPage() { DataContext = configuration };
        }

        protected override UserControl CreateSummaryPresenter(RavenDbSourceAdapterConfiguration configuration)
        {
            return new RavenDbSourceAdapterConfigurationSummaryPage { DataContext = configuration };
        }

        protected override RavenDbSourceAdapterConfiguration CreateValidatableConfiguration()
        {
            return new RavenDbSourceAdapterConfiguration();
        }

        protected override void PopulateCommandLineArguments(RavenDbSourceAdapterConfiguration configuration, IDictionary<string, string> arguments)
        {
            Guard.NotNull("configuration", configuration);
            Guard.NotNull("arguments", arguments);

            arguments.Add(RavenDbSourceAdapterConfiguration.ConnectionStringPropertyName, configuration.ConnectionString);

            if (configuration.UseQueryFile)
            {
                if (!String.IsNullOrEmpty(configuration.QueryFile))
                    arguments.Add(RavenDbSourceAdapterConfiguration.QueryFilePropertyName, configuration.QueryFile);
            }
            else
            {
                if (!String.IsNullOrEmpty(configuration.Query))
                    arguments.Add(RavenDbSourceAdapterConfiguration.QueryPropertyName, configuration.Query);
            }

            if (!String.IsNullOrEmpty(configuration.Index))
            {
                arguments.Add(RavenDbSourceAdapterConfiguration.IndexPropertyName, configuration.Index);
            }

            if (configuration.ExcludeId)
                arguments.Add(RavenDbSourceAdapterConfiguration.ExcludeIdPropertyName, null);
        }
    }
}
