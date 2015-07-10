using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.HBase.Wpf.Source
{
    sealed class HBaseSourceAdapterInternalConfigurationProvider : DataAdapterValidatableConfigurationProviderBase<HBaseSourceAdapterConfiguration>
    {
        protected override UserControl CreatePresenter(HBaseSourceAdapterConfiguration configuration)
        {
            return new HBaseSourceAdapterConfigurationPage() { DataContext = configuration };
        }

        protected override UserControl CreateSummaryPresenter(HBaseSourceAdapterConfiguration configuration)
        {
            return new HBaseSourceAdapterConfigurationSummaryPage { DataContext = configuration };
        }

        protected override HBaseSourceAdapterConfiguration CreateValidatableConfiguration()
        {
            return new HBaseSourceAdapterConfiguration();
        }

        protected override void PopulateCommandLineArguments(HBaseSourceAdapterConfiguration configuration, IDictionary<string, string> arguments)
        {
            Guard.NotNull("configuration", configuration);
            Guard.NotNull("arguments", arguments);

            arguments.Add(HBaseSourceAdapterConfiguration.ConnectionStringPropertyName, configuration.ConnectionString);
            arguments.Add(HBaseSourceAdapterConfiguration.TablePropertyName, configuration.Table);

            if (configuration.UseFilterFile)
            {
                if (!String.IsNullOrEmpty(configuration.FilterFile))
                    arguments.Add(HBaseSourceAdapterConfiguration.FilterFilePropertyName, configuration.FilterFile);
            }
            else
            {
                if (!String.IsNullOrEmpty(configuration.Filter))
                    arguments.Add(HBaseSourceAdapterConfiguration.FilterPropertyName, configuration.Filter);
            }

            if (configuration.ExcludeId)
                arguments.Add(HBaseSourceAdapterConfiguration.ExcludeIdPropertyName, null);

            if (configuration.BatchSize.HasValue && configuration.BatchSize.Value != Defaults.Current.SourceBatchSize)
                arguments.Add(
                    HBaseSourceAdapterConfiguration.BatchSizePropertyName,
                    configuration.BatchSize.Value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
