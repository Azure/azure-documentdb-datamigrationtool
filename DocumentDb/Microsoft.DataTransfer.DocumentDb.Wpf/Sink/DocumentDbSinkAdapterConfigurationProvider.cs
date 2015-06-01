using Microsoft.DataTransfer.DocumentDb.Wpf.Shared;
using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Sink
{
    abstract class DocumentDbSinkAdapterConfigurationProvider<TConfiguration> : DocumentDbAdapterConfigurationProvider<TConfiguration>
        where TConfiguration : DocumentDbSinkAdapterConfiguration
    {
        protected override void PopulateCommandLineArguments(TConfiguration configuration, IDictionary<string, string> arguments)
        {
            base.PopulateCommandLineArguments(configuration, arguments);

            arguments.Add(DocumentDbSinkAdapterConfiguration.CollectionPropertyName, AsCollectionArgument(configuration.Collection));

            if (!String.IsNullOrEmpty(configuration.PartitionKey))
                arguments.Add(DocumentDbSinkAdapterConfiguration.PartitionKeyPropertyName, configuration.PartitionKey);

            if (configuration.CollectionTier.HasValue && configuration.CollectionTier.Value != Defaults.Current.SinkCollectionTier)
                arguments.Add(DocumentDbSinkAdapterConfiguration.CollectionTierPropertyName, configuration.CollectionTier.Value.ToString());

            if (!String.IsNullOrEmpty(configuration.IdField))
                arguments.Add(DocumentDbSinkAdapterConfiguration.IdFieldPropertyName, configuration.IdField);

            if (configuration.DisableIdGeneration)
                arguments.Add(DocumentDbSinkAdapterConfiguration.DisableIdGenerationPropertyName, null);

            if (configuration.Dates.HasValue && configuration.Dates.Value != Defaults.Current.SinkDateTimeHandling)
                arguments.Add(DocumentDbSinkAdapterConfiguration.DatesPropertyName, configuration.Dates.Value.ToString());
        }
    }
}
