using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.FirebaseJsonFile.Wpf.Source
{
    class FirebaseJsonFileSourceAdapterInternalConfigurationProvider : DataAdapterValidatableConfigurationProviderBase<FirebaseJsonFileSourceAdapterConfiguration>
    {
        protected override UserControl CreatePresenter(FirebaseJsonFileSourceAdapterConfiguration configuration)
        {
            return new FirebaseJsonFileSourceAdapterConfigurationPage { DataContext = configuration };
        }

        protected override UserControl CreateSummaryPresenter(FirebaseJsonFileSourceAdapterConfiguration configuration)
        {
            return new FirebaseJsonFileSourceAdapterConfigurationSummaryPage { DataContext = configuration };
        }

        protected override FirebaseJsonFileSourceAdapterConfiguration CreateValidatableConfiguration()
        {
            return new FirebaseJsonFileSourceAdapterConfiguration();
        }

        protected override void PopulateCommandLineArguments(FirebaseJsonFileSourceAdapterConfiguration configuration, IDictionary<string, string> arguments)
        {
            Guard.NotNull("configuration", configuration);
            Guard.NotNull("arguments", arguments);

            arguments.Add(FirebaseJsonFileSourceAdapterConfiguration.FilesPropertyName, AsCollectionArgument(configuration.Files));

            if (configuration.Decompress)
                arguments.Add(FirebaseJsonFileSourceAdapterConfiguration.DecompressPropertyName, null);

            if (!string.IsNullOrEmpty(configuration.Node))
                arguments.Add(FirebaseJsonFileSourceAdapterConfiguration.NodePropertyName, configuration.Node);

            if (!string.IsNullOrEmpty(configuration.IdField))
                arguments.Add(FirebaseJsonFileSourceAdapterConfiguration.IdFieldPropertyName, configuration.IdField);
        }
    }
}
