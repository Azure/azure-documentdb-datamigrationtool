using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.DocumentDb.Client;
using Microsoft.DataTransfer.DocumentDb.Transformation;
using Microsoft.DataTransfer.Extensibility;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Parallel
{
    sealed class DocumentDbParallelSinkAdapterInternalFactory : DocumentDbSinkAdapterFactoryBase<IDocumentDbParallelSinkAdapterConfiguration>
    {
        public override string Description
        {
            get { return Resources.ParallelSinkDescription; }
        }

        protected override async Task<IDataSinkAdapter> CreateAsync(DocumentDbClient client,
            IDataItemTransformation transformation, IDocumentDbParallelSinkAdapterConfiguration configuration)
        {
            var sink = new DocumentDbParallelSinkAdapter(client, transformation, GetInstanceConfiguration(configuration));
            await sink.InitializeAsync();
            return sink;
        }

        private static DocumentDbParallelSinkAdapterInstanceConfiguration GetInstanceConfiguration(IDocumentDbParallelSinkAdapterConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

            return new DocumentDbParallelSinkAdapterInstanceConfiguration
            {
                CollectionName = configuration.Collection,
                CollectionTier = GetValueOrDefault(configuration.CollectionTier, Defaults.Current.SinkCollectionTier),
                DisableIdGeneration = configuration.DisableIdGeneration,
                NumberOfParallelRequests = GetValueOrDefault(configuration.ParallelRequests,
                    Defaults.Current.ParallelSinkNumberOfParallelRequests, Errors.InvalidNumberOfParallelRequests)
            };
        }
    }
}
