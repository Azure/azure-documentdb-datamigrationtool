using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.IO;
using Microsoft.DataTransfer.DocumentDb.Sink.Substitutions;
using Microsoft.DataTransfer.DocumentDb.Sink.Substitutions.Range;
using Microsoft.DataTransfer.DocumentDb.Transformation;
using Microsoft.DataTransfer.Extensibility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    sealed class DocumentDbBulkSinkAdapterInternalFactory : DocumentDbSinkAdapterFactoryBase<IDocumentDbBulkSinkAdapterConfiguration>
    {
        private static readonly ISubstitutionResolver substitutions = new RangeSubstitutionResolver();

        public override string Description
        {
            get { return Resources.BulkSinkDescription; }
        }

        protected override async Task<IDataSinkAdapter> CreateAsync(IDataTransferContext context, IDataItemTransformation transformation,
            IDocumentDbBulkSinkAdapterConfiguration configuration, CancellationToken cancellation)
        {
            var collectionNames = ResolveCollectionNames(configuration.Collection);
            if (!collectionNames.Any())
                throw Errors.CollectionNameMissing();

            var sink = new DocumentDbBulkSinkAdapterDispatcher(
                CreateClient(configuration, context, collectionNames.Count() > 1, null),
                transformation, GetInstanceConfiguration(configuration, collectionNames));

            await sink.InitializeAsync(cancellation);

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
                CollectionThroughput = GetValueOrDefault(configuration.CollectionThroughput,
                    Defaults.Current.SinkCollectionThroughput, Errors.InvalidCollectionThroughput),
                IndexingPolicy = GetIndexingPolicy(configuration),
                DisableIdGeneration = configuration.DisableIdGeneration,
                UpdateExisting = configuration.UpdateExisting,
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

        private static string[] ResolveCollectionNames(IEnumerable<string> collectionNamePatterns)
        {
            return collectionNamePatterns.AsParallel().SelectMany(p => substitutions.Resolve(p)).Distinct().ToArray();
        }
    }
}
