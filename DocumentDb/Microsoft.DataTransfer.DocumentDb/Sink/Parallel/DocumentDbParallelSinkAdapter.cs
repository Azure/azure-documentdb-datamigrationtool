using Microsoft.DataTransfer.Basics.Threading;
using Microsoft.DataTransfer.DocumentDb.Client;
using Microsoft.DataTransfer.DocumentDb.Shared;
using Microsoft.DataTransfer.DocumentDb.Transformation;
using Microsoft.DataTransfer.Extensibility;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Parallel
{
    sealed class DocumentDbParallelSinkAdapter : DocumentDbAdapterBase<IDocumentDbWriteClient, IDocumentDbParallelSinkAdapterInstanceConfiguration>, IDataSinkAdapter
    {
        private string collectionLink;

        public int MaxDegreeOfParallelism
        {
            get { return Configuration.NumberOfParallelRequests; }
        }

        public DocumentDbParallelSinkAdapter(IDocumentDbWriteClient client, IDataItemTransformation transformation, IDocumentDbParallelSinkAdapterInstanceConfiguration configuration)
            : base(client, transformation, configuration) { }

        public async Task InitializeAsync(CancellationToken cancellation)
        {
            collectionLink = await Client.GetOrCreateCollectionAsync(
                Configuration.Collection, Configuration.PartitionKey,
                Configuration.CollectionThroughput, Configuration.IndexingPolicy,
                cancellation);
        }

        public Task WriteAsync(IDataItem dataItem, CancellationToken cancellation)
        {
            if (String.IsNullOrEmpty(collectionLink))
                throw Errors.SinkIsNotInitialized();

            dataItem = Transformation.Transform(dataItem);

            var dataItemSurrogate = new DataItemSurrogate(dataItem);

            return Configuration.UpdateExisting
                ? Client.UpsertDocumentAsync(collectionLink, dataItemSurrogate, Configuration.DisableIdGeneration)
                : Client.CreateDocumentAsync(collectionLink, dataItemSurrogate, Configuration.DisableIdGeneration);
        }

        public Task CompleteAsync(CancellationToken cancellation)
        {
            return TaskHelper.NoOp;
        }
    }
}
