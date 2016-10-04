using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Extensibility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Sink.Bulk
{
    sealed class DocumentDbBulkSinkAdapterInternalConfigurationProvider : DocumentDbSinkAdapterConfigurationProvider<DocumentDbBulkSinkAdapterConfiguration>
    {
        public DocumentDbBulkSinkAdapterInternalConfigurationProvider(IImportSharedStorage sharedStorage)
            : base(sharedStorage) { }

        protected override UserControl CreatePresenter(DocumentDbBulkSinkAdapterConfiguration configuration)
        {
            return new DocumentDbBulkSinkAdapterConfigurationPage { DataContext = configuration };
        }

        protected override UserControl CreateSummaryPresenter(DocumentDbBulkSinkAdapterConfiguration configuration)
        {
            return new DocumentDbBulkSinkAdapterConfigurationSummaryPage { DataContext = configuration };
        }

        protected override DocumentDbBulkSinkAdapterConfiguration CreateValidatableConfiguration()
        {
            return new DocumentDbBulkSinkAdapterConfiguration(GetSharedConfiguration());
        }

        protected override void PopulateCommandLineArguments(DocumentDbBulkSinkAdapterConfiguration configuration, IDictionary<string, string> arguments)
        {
            base.PopulateCommandLineArguments(configuration, arguments);

            Guard.NotNull("configuration", configuration);
            Guard.NotNull("arguments", arguments);

            arguments.Add(DocumentDbBulkSinkAdapterConfiguration.CollectionPropertyName, AsCollectionArgument(configuration.Collection));

            if (!String.IsNullOrEmpty(configuration.PartitionKey))
                arguments.Add(DocumentDbBulkSinkAdapterConfiguration.PartitionKeyPropertyName, configuration.PartitionKey);

            if (!String.IsNullOrEmpty(configuration.StoredProcFile))
                arguments.Add(DocumentDbBulkSinkAdapterConfiguration.StoredProcFilePropertyName, configuration.StoredProcFile);

            if (configuration.BatchSize.HasValue && configuration.BatchSize.Value != Defaults.Current.BulkSinkBatchSize)
                arguments.Add(
                    DocumentDbBulkSinkAdapterConfiguration.BatchSizePropertyName,
                    configuration.BatchSize.Value.ToString(CultureInfo.InvariantCulture));

            if (configuration.MaxScriptSize.HasValue && configuration.MaxScriptSize.Value != Defaults.Current.BulkSinkMaxScriptSize)
                arguments.Add(
                    DocumentDbBulkSinkAdapterConfiguration.MaxScriptSizePropertyName,
                    configuration.MaxScriptSize.Value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
