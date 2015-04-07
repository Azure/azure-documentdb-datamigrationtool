using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.DocumentDb.Client;
using Microsoft.DataTransfer.DocumentDb.Transformation;
using Microsoft.DataTransfer.Extensibility;
using System;
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

        protected override async Task<IDataSinkAdapter> CreateAsync(DocumentDbClient client,
            IDataItemTransformation transformation, IDocumentDbBulkSinkAdapterConfiguration configuration)
        {
            var sink = new DocumentDbBulkSinkAdapter(client, transformation, GetInstanceConfiguration(configuration));
            await sink.InitializeAsync();
            return sink;
        }

        private static DocumentDbBulkSinkAdapterInstanceConfiguration GetInstanceConfiguration(IDocumentDbBulkSinkAdapterConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

            return new DocumentDbBulkSinkAdapterInstanceConfiguration
            {
                CollectionName = configuration.Collection,
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
