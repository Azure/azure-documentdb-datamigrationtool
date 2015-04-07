using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.MongoDb.Wpf.Source.Online
{
    sealed class MongoDbSourceAdapterInternalConfigurationProvider : DataAdapterValidatableConfigurationProviderBase<MongoDbSourceAdapterConfiguration>
    {
        protected override UserControl CreatePresenter(MongoDbSourceAdapterConfiguration configuration)
        {
            return new MongoDbSourceAdapterConfigurationPage() { DataContext = configuration };
        }

        protected override UserControl CreateSummaryPresenter(MongoDbSourceAdapterConfiguration configuration)
        {
            return new MongoDbSourceAdapterConfigurationSummaryPage { DataContext = configuration };
        }

        protected override MongoDbSourceAdapterConfiguration CreateValidatableConfiguration()
        {
            return new MongoDbSourceAdapterConfiguration();
        }

        protected override void PopulateCommandLineArguments(MongoDbSourceAdapterConfiguration configuration, IDictionary<string, string> arguments)
        {
            Guard.NotNull("configuration", configuration);
            Guard.NotNull("arguments", arguments);

            arguments.Add(MongoDbSourceAdapterConfiguration.ConnectionStringPropertyName, configuration.ConnectionString);
            arguments.Add(MongoDbSourceAdapterConfiguration.CollectionPropertyName, configuration.Collection);

            if (configuration.UseQueryFile)
            {
                if (!String.IsNullOrEmpty(configuration.QueryFile))
                    arguments.Add(MongoDbSourceAdapterConfiguration.QueryFilePropertyName, configuration.QueryFile);
            }
            else 
            {
                if (!String.IsNullOrEmpty(configuration.Query))
                    arguments.Add(MongoDbSourceAdapterConfiguration.QueryPropertyName, configuration.Query);
            }

            if (configuration.UseProjectionFile)
            {
                if (!String.IsNullOrEmpty(configuration.ProjectionFile))
                    arguments.Add(MongoDbSourceAdapterConfiguration.ProjectionFilePropertyName, configuration.ProjectionFile);
            }
            else 
            {
                if (!String.IsNullOrEmpty(configuration.Projection))
                    arguments.Add(MongoDbSourceAdapterConfiguration.ProjectionPropertyName, configuration.Projection);
            }
        }
    }
}
