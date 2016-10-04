using Microsoft.Azure.Documents.Client;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.DocumentDb.Client;
using Microsoft.DataTransfer.DocumentDb.Client.PartitionResolvers;
using Microsoft.DataTransfer.DocumentDb.Shared;
using Microsoft.DataTransfer.DocumentDb.Transformation;
using Microsoft.DataTransfer.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    sealed class DocumentDbBulkSinkAdapterDispatcher : DocumentDbAdapterBase<IDocumentDbWriteClient, IDocumentDbBulkSinkAdapterDispatcherConfiguration>, IDataSinkAdapter
    {
        private const string BulkImportStoredProcPrefix = "BulkImport";

        private IDictionary<string, IDataSinkAdapter> dataAdapters;
        private IPartitionResolver partitionResolver;

        public int MaxDegreeOfParallelism
        {
            get { return dataAdapters.Values.Sum(a => a.MaxDegreeOfParallelism); }
        }

        public DocumentDbBulkSinkAdapterDispatcher(IDocumentDbWriteClient client, IDataItemTransformation transformation, IDocumentDbBulkSinkAdapterDispatcherConfiguration configuration)
            : base(client, transformation, configuration)
        {
            dataAdapters = new Dictionary<string, IDataSinkAdapter>();
        }

        public async Task InitializeAsync(CancellationToken cancellation)
        {
            var initializationTasks = new List<Task>();
            var storedProcName = BulkImportStoredProcPrefix + Guid.NewGuid().ToString("N");
            var collections = Configuration.Collections.ToList();

            foreach (var collectionName in collections)
            {
                var adapter = new DocumentDbBulkSinkAdapter(Client, PassThroughTransformation.Instance,
                    CreateInstanceConfiguration(collectionName, storedProcName));
                dataAdapters.Add(collectionName, adapter);
                initializationTasks.Add(adapter.InitializeAsync(cancellation));
            }

            partitionResolver = PartitionResolverFactory.Instance.Create(Configuration.PartitionKey, collections);

            await Task.WhenAll(initializationTasks);
        }

        private IDocumentDbBulkSinkAdapterInstanceConfiguration CreateInstanceConfiguration(string collectionName, string storedProcName)
        {
            return new DocumentDbBulkSinkAdapterInstanceConfiguration
            {
                Collection = collectionName,
                CollectionThroughput = Configuration.CollectionThroughput,
                IndexingPolicy = Configuration.IndexingPolicy,
                DisableIdGeneration = Configuration.DisableIdGeneration,
                UpdateExisting = Configuration.UpdateExisting,
                StoredProcName = storedProcName,
                StoredProcBody = Configuration.StoredProcBody,
                BatchSize = Configuration.BatchSize,
                MaxScriptSize = Configuration.MaxScriptSize
            };
        }

        public Task WriteAsync(IDataItem dataItem, CancellationToken cancellation)
        {
            if (partitionResolver == null)
                throw Errors.SinkIsNotInitialized();

            dataItem = Transformation.Transform(dataItem);
            var collectionName = partitionResolver.ResolveForCreate(partitionResolver.GetPartitionKey(dataItem));

            IDataSinkAdapter adapter;
            if (!dataAdapters.TryGetValue(collectionName, out adapter))
                throw Errors.UnexpectedPartitionCollection(collectionName);

            return adapter.WriteAsync(dataItem, cancellation);
        }

        public async Task CompleteAsync(CancellationToken cancellation)
        {
            var completeTasks = new List<Task>(dataAdapters.Count);

            foreach (var adapter in dataAdapters.Values)
                completeTasks.Add(adapter.CompleteAsync(cancellation));

            await Task.WhenAll(completeTasks);
        }

        public override void Dispose()
        {
            var adapters = Interlocked.Exchange(ref dataAdapters, null);

            if (adapters != null)
            {
                foreach (var adapter in adapters.Values)
                {
                    var adapterCopy = adapter;
                    TrashCan.Throw(ref adapterCopy);
                }
            }

            base.Dispose();
        }
    }
}
