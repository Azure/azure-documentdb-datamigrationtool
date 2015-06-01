using Microsoft.DataTransfer.AzureTable.Wpf.Shared;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.AzureTable.Wpf.Source
{
    sealed class AzureTableSourceAdapterInternalConfigurationProvider : AzureTableAdapterConfigurationProvider<AzureTableSourceAdapterConfiguration>
    {
        protected override UserControl CreatePresenter(AzureTableSourceAdapterConfiguration configuration)
        {
            return new AzureTableSourceAdapterConfigurationPage() { DataContext = configuration };
        }

        protected override UserControl CreateSummaryPresenter(AzureTableSourceAdapterConfiguration configuration)
        {
            return new AzureTableSourceAdapterConfigurationSummaryPage { DataContext = configuration };
        }

        protected override AzureTableSourceAdapterConfiguration CreateValidatableConfiguration()
        {
            return new AzureTableSourceAdapterConfiguration();
        }

        protected override void PopulateCommandLineArguments(AzureTableSourceAdapterConfiguration configuration, IDictionary<string, string> arguments)
        {
            base.PopulateCommandLineArguments(configuration, arguments);

            Guard.NotNull("configuration", configuration);
            Guard.NotNull("arguments", arguments);

            arguments.Add(AzureTableSourceAdapterConfiguration.TablePropertyName, configuration.Table);

            if (configuration.InternalFields.HasValue && configuration.InternalFields.Value != Defaults.Current.SourceInternalFields)
                arguments.Add(
                    AzureTableSourceAdapterConfiguration.InternalFieldsPropertyName,
                    configuration.InternalFields.Value.ToString());

            if (!String.IsNullOrEmpty(configuration.Filter))
                arguments.Add(AzureTableSourceAdapterConfiguration.FilterPropertyName, configuration.Filter);

            if (configuration.Projection != null && configuration.Projection.Any())
                arguments.Add(AzureTableSourceAdapterConfiguration.ProjectionPropertyName, AsCollectionArgument(configuration.Projection));
        }
    }
}
