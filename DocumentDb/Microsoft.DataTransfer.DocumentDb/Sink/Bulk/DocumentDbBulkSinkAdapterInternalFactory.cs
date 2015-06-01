using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.IO;
using Microsoft.DataTransfer.DocumentDb.Client;
using Microsoft.DataTransfer.DocumentDb.Transformation;
using Microsoft.DataTransfer.Extensibility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    sealed class DocumentDbBulkSinkAdapterInternalFactory : DocumentDbSinkAdapterFactoryBase<IDocumentDbBulkSinkAdapterConfiguration>
    {
        public override string Description
        {
            get { return Resources.BulkSinkDescription; }
        }

        protected override async Task<IDataSinkAdapter> CreateAsync(DocumentDbClient client, IDataItemTransformation transformation,
            IDocumentDbBulkSinkAdapterConfiguration configuration, IEnumerable<string> collectionNames)
        {
            var sink = new DocumentDbBulkSinkAdapterDispatcher(client, transformation, GetInstanceConfiguration(configuration, collectionNames));
            await sink.InitializeAsync();
            return sink;
        }

        private static DocumentDbBulkSinkAdapterDispatcherConfiguration GetInstanceConfiguration(
            IDocumentDbBulkSinkAdapterConfiguration configuration, IEnumerable<string> collectionNames)
        {
            Guard.NotNull("configuration", configuration);

            return new DocumentDbBulkSinkAdapterDispatcherConfiguration
            {
                Collections = collectionNames,
                PartitionKey = configuration.PartitionKey,
                CollectionTier = GetValueOrDefault(configuration.CollectionTier, Defaults.Current.SinkCollectionTier),
                DisableIdGeneration = configuration.DisableIdGeneration,
                StoredProcBody = GetStoredProcBody(configuration.StoredProcFile),
                BatchSize = GetValueOrDefault(configuration.BatchSize,
                    Defaults.Current.BulkSinkBatchSize, Errors.InvalidBatchSize),
                MaxScriptSize = GetValueOrDefault(configuration.MaxScriptSize,
                    Defaults.Current.BulkSinkMaxScriptSize, Errors.InvalidMaxScriptSize)
            };
        }

        private static string GetStoredProcBody(string storedProcFile)
        {
            return String.IsNullOrEmpty(storedProcFile)
                ? File.ReadAllText(PathHelper.Combine(AppDomain.CurrentDomain.BaseDirectory, Defaults.Current.BulkSinkStoredProcFile))
                : File.ReadAllText(storedProcFile);
        }
    }
}
