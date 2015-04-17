using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;

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

            arguments.Add(RavenDbSourceAdapterConfiguration.CollectionsPropertyName, String.Join(";", configuration.Collections.Select(f => f.Replace(";", @"\;"))));
        }
    }
}
