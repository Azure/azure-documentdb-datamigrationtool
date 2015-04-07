using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.Sql.Wpf.Source
{
    sealed class SqlDataSourceAdapterInternalConfigurationProvider : DataAdapterValidatableConfigurationProviderBase<SqlDataSourceAdapterConfiguration>
    {
        protected override UserControl CreatePresenter(SqlDataSourceAdapterConfiguration configuration)
        {
            return new SqlDataSourceAdapterConfigurationPage() { DataContext = configuration };
        }

        protected override UserControl CreateSummaryPresenter(SqlDataSourceAdapterConfiguration configuration)
        {
            return new SqlDataSourceAdapterConfigurationSummaryPage { DataContext = configuration };
        }

        protected override SqlDataSourceAdapterConfiguration CreateValidatableConfiguration()
        {
            return new SqlDataSourceAdapterConfiguration();
        }

        protected override void PopulateCommandLineArguments(SqlDataSourceAdapterConfiguration configuration, IDictionary<string, string> arguments)
        {
            Guard.NotNull("configuration", configuration);
            Guard.NotNull("arguments", arguments);

            arguments.Add(SqlDataSourceAdapterConfiguration.ConnectionStringPropertyName, configuration.ConnectionString);

            if (configuration.UseQueryFile)
            {
                if (!String.IsNullOrEmpty(configuration.QueryFile))
                    arguments.Add(SqlDataSourceAdapterConfiguration.QueryFilePropertyName, configuration.QueryFile);
            }
            else
            {
                if (!String.IsNullOrEmpty(configuration.Query))
                    arguments.Add(SqlDataSourceAdapterConfiguration.QueryPropertyName, configuration.Query);
            }
            
            if (!String.IsNullOrEmpty(configuration.NestingSeparator))
                arguments.Add(SqlDataSourceAdapterConfiguration.NestingSeparatorPropertyName, configuration.NestingSeparator);
        }
    }
}
