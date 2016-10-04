using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.JsonFile.Wpf.Source
{
    sealed class JsonFileSourceAdapterInternalConfigurationProvider : DataAdapterValidatableConfigurationProviderBase<JsonFileSourceAdapterConfiguration>
    {
        protected override UserControl CreatePresenter(JsonFileSourceAdapterConfiguration configuration)
        {
            return new JsonFileSourceAdapterConfigurationPage() { DataContext = configuration };
        }

        protected override UserControl CreateSummaryPresenter(JsonFileSourceAdapterConfiguration configuration)
        {
            return new JsonFileSourceAdapterConfigurationSummaryPage { DataContext = configuration };
        }

        protected override JsonFileSourceAdapterConfiguration CreateValidatableConfiguration()
        {
            return new JsonFileSourceAdapterConfiguration();
        }

        protected override void PopulateCommandLineArguments(JsonFileSourceAdapterConfiguration configuration, IDictionary<string, string> arguments)
        {
            Guard.NotNull("configuration", configuration);
            Guard.NotNull("arguments", arguments);

            arguments.Add(JsonFileSourceAdapterConfiguration.FilesPropertyName, AsCollectionArgument(configuration.Files));

            if (configuration.Decompress)
                arguments.Add(JsonFileSourceAdapterConfiguration.DecompressPropertyName, null);
        }
    }
}
