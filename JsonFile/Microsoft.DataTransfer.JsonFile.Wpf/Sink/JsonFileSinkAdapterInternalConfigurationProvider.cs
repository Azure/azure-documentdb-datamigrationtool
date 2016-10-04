using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.JsonFile.Wpf.Sink
{
    sealed class JsonFileSinkAdapterInternalConfigurationProvider : DataAdapterValidatableConfigurationProviderBase<JsonFileSinkAdapterConfiguration>
    {
        protected override UserControl CreatePresenter(JsonFileSinkAdapterConfiguration configuration)
        {
            return new JsonFileSinkAdapterConfigurationPage { DataContext = configuration };
        }

        protected override UserControl CreateSummaryPresenter(JsonFileSinkAdapterConfiguration configuration)
        {
            return new JsonFileSinkAdapterConfigurationSummaryPage { DataContext = configuration };
        }

        protected override JsonFileSinkAdapterConfiguration CreateValidatableConfiguration()
        {
            return new JsonFileSinkAdapterConfiguration();
        }

        protected override void PopulateCommandLineArguments(JsonFileSinkAdapterConfiguration configuration, IDictionary<string, string> arguments)
        {
            Guard.NotNull("configuration", configuration);
            Guard.NotNull("arguments", arguments);

            arguments.Add(JsonFileSinkAdapterConfiguration.FilePropertyName, configuration.File);

            if (configuration.Prettify)
                arguments.Add(JsonFileSinkAdapterConfiguration.PrettifyPropertyName, null);

            if (configuration.Overwrite)
                arguments.Add(JsonFileSinkAdapterConfiguration.OverwritePropertyName, null);

            if (configuration.Compress)
                arguments.Add(JsonFileSinkAdapterConfiguration.CompressPropertyName, null);
        }
    }
}
