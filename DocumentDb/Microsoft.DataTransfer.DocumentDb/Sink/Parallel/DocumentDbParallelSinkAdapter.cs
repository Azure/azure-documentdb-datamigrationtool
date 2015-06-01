using Microsoft.Azure.Documents.Client;
using Microsoft.DataTransfer.Basics.Threading;
using Microsoft.DataTransfer.DocumentDb.Client;
using Microsoft.DataTransfer.DocumentDb.Client.PartitionResolvers;
using Microsoft.DataTransfer.DocumentDb.Shared;
using Microsoft.DataTransfer.DocumentDb.Transformation;
using Microsoft.DataTransfer.Extensibility;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Parallel
{
    sealed class DocumentDbParallelSinkAdapter : DocumentDbAdapterBase<IDocumentDbWriteClient, IDocumentDbParallelSinkAdapterInstanceConfiguration>, IDataSinkAdapter
    {
        private IPartitionResolver partitionResolver;

        public int MaxDegreeOfParallelism
        {
            get { return Configuration.NumberOfParallelRequests; }
        }

        public DocumentDbParallelSinkAdapter(IDocumentDbWriteClient client, IDataItemTransformation transformation, IDocumentDbParallelSinkAdapterInstanceConfiguration configuration)
            : base(client, transformation, configuration) { }

        public async Task InitializeAsync()
        {
            var collectionLinks = new List<string>();

            foreach (var collection in Configuration.Collections)
                collectionLinks.Add(await Client.GetOrCreateCollectionAsync(collection, Configuration.CollectionTier));

            partitionResolver = PartitionResolverFactory.Instance.Create(Configuration.PartitionKey, collectionLinks);
        }

        public Task WriteAsync(IDataItem dataItem, CancellationToken cancellation)
        {
            if (partitionResolver == null)
                throw Errors.SinkIsNotInitialized();

            dataItem = Transformation.Transform(dataItem);
            var partitionKey = partitionResolver.GetPartitionKey(dataItem);

            return Client.CreateDocumentAsync(
                partitionResolver.ResolveForCreate(partitionKey),
                new DataItemSurrogate(dataItem),
                Configuration.DisableIdGeneration);
        }

        public Task CompleteAsync(CancellationToken cancellation)
        {
            return TaskHelper.NoOp;
        }
    }
}
