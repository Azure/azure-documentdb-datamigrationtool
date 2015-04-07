using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;
using System;
using System.Collections.Generic;
using System.Linq;
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

            arguments.Add(CsvFileSourceAdapterConfiguration.FilesPropertyName, String.Join(";", configuration.Files.Select(f => f.Replace(";", @"\;"))));

            if (!String.IsNullOrEmpty(configuration.NestingSeparator))
                arguments.Add(CsvFileSourceAdapterConfiguration.NestingSeparatorPropertyName, configuration.NestingSeparator);
        }
    }
}
