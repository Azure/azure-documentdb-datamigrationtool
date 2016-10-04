using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Extensibility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Sink.Parallel
{
    sealed class DocumentDbParallelSinkAdapterInternalConfigurationProvider : DocumentDbSinkAdapterConfigurationProvider<DocumentDbParallelSinkAdapterConfiguration>
    {
        public DocumentDbParallelSinkAdapterInternalConfigurationProvider(IImportSharedStorage sharedStorage)
            : base(sharedStorage) { }

        protected override UserControl CreatePresenter(DocumentDbParallelSinkAdapterConfiguration configuration)
        {
            return new DocumentDbParallelSinkAdapterConfigurationPage { DataContext = configuration };
        }

        protected override UserControl CreateSummaryPresenter(DocumentDbParallelSinkAdapterConfiguration configuration)
        {
            return new DocumentDbParallelSinkAdapterConfigurationSummaryPage { DataContext = configuration };
        }

        protected override DocumentDbParallelSinkAdapterConfiguration CreateValidatableConfiguration()
        {
            return new DocumentDbParallelSinkAdapterConfiguration(GetSharedConfiguration());
        }

        protected override void PopulateCommandLineArguments(DocumentDbParallelSinkAdapterConfiguration configuration, IDictionary<string, string> arguments)
        {
            base.PopulateCommandLineArguments(configuration, arguments);

            Guard.NotNull("configuration", configuration);
            Guard.NotNull("arguments", arguments);

            arguments.Add(DocumentDbParallelSinkAdapterConfiguration.CollectionPropertyName, configuration.Collection);

            if (!String.IsNullOrEmpty(configuration.PartitionKey))
                arguments.Add(DocumentDbParallelSinkAdapterConfiguration.PartitionKeyPropertyName, configuration.PartitionKey);

            if (configuration.ParallelRequests.HasValue && configuration.ParallelRequests.Value != Defaults.Current.ParallelSinkNumberOfParallelRequests)
                arguments.Add(
                    DocumentDbParallelSinkAdapterConfiguration.ParallelRequestsPropertyName,
                    configuration.ParallelRequests.Value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
