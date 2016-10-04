using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.CsvFile.Wpf.Source
{
    sealed class CsvFileSourceAdapterInternalConfigurationProvider : DataAdapterValidatableConfigurationProviderBase<CsvFileSourceAdapterConfiguration>
    {
        protected override UserControl CreatePresenter(CsvFileSourceAdapterConfiguration configuration)
        {
            return new CsvFileSourceAdapterConfigurationPage() { DataContext = configuration };
        }

        protected override UserControl CreateSummaryPresenter(CsvFileSourceAdapterConfiguration configuration)
        {
            return new CsvFileSourceAdapterConfigurationSummaryPage { DataContext = configuration };
        }

        protected override CsvFileSourceAdapterConfiguration CreateValidatableConfiguration()
        {
            return new CsvFileSourceAdapterConfiguration();
        }

        protected override void PopulateCommandLineArguments(CsvFileSourceAdapterConfiguration configuration, IDictionary<string, string> arguments)
        {
            Guard.NotNull("configuration", configuration);
            Guard.NotNull("arguments", arguments);

            arguments.Add(CsvFileSourceAdapterConfiguration.FilesPropertyName, AsCollectionArgument(configuration.Files));

            if (!String.IsNullOrEmpty(configuration.NestingSeparator))
                arguments.Add(CsvFileSourceAdapterConfiguration.NestingSeparatorPropertyName, configuration.NestingSeparator);

            if (configuration.TrimQuoted)
                arguments.Add(CsvFileSourceAdapterConfiguration.TrimQuotedPropertyName, null);

            if (configuration.NoUnquotedNulls)
                arguments.Add(CsvFileSourceAdapterConfiguration.NoUnquotedNullsPropertyName, null);

            if (configuration.UseRegionalSettings)
                arguments.Add(CsvFileSourceAdapterConfiguration.UseRegionalSettingsPropertyName, null);

            if (configuration.Decompress)
                arguments.Add(CsvFileSourceAdapterConfiguration.DecompressPropertyName, null);
        }
    }
}
