using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.DocumentDb.Transformation;
using Microsoft.DataTransfer.Extensibility;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Parallel
{
    sealed class DocumentDbParallelSinkAdapterInternalFactory : DocumentDbSinkAdapterFactoryBase<IDocumentDbParallelSinkAdapterConfiguration>
    {
        public override string Description
        {
            get { return Resources.ParallelSinkDescription; }
        }

        protected override async Task<IDataSinkAdapter> CreateAsync(IDataTransferContext context, IDataItemTransformation transformation,
            IDocumentDbParallelSinkAdapterConfiguration configuration, CancellationToken cancellation)
        {
            if (String.IsNullOrEmpty(configuration.Collection))
                throw Errors.CollectionNameMissing();

            var instanceConfiguration = GetInstanceConfiguration(configuration);

            var sink = new DocumentDbParallelSinkAdapter(
                CreateClient(configuration, context, false, instanceConfiguration.NumberOfParallelRequests),
                transformation,
                instanceConfiguration);

            await sink.InitializeAsync(cancellation);

            return sink;
        }

        private static DocumentDbParallelSinkAdapterInstanceConfiguration GetInstanceConfiguration(
            IDocumentDbParallelSinkAdapterConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

            return new DocumentDbParallelSinkAdapterInstanceConfiguration
            {
                Collection = configuration.Collection,
                PartitionKey = configuration.PartitionKey,
                CollectionThroughput = GetValueOrDefault(configuration.CollectionThroughput,
                    Defaults.Current.SinkCollectionThroughput, Errors.InvalidCollectionThroughput),
                IndexingPolicy = GetIndexingPolicy(configuration),
                DisableIdGeneration = configuration.DisableIdGeneration,
                UpdateExisting = configuration.UpdateExisting,
                NumberOfParallelRequests = GetValueOrDefault(configuration.ParallelRequests,
                    Defaults.Current.ParallelSinkNumberOfParallelRequests, Errors.InvalidNumberOfParallelRequests)
            };
        }
    }
}
