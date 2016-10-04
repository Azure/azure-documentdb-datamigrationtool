using Microsoft.Azure.Documents;
using Microsoft.DataTransfer.Basics.Threading;
using Microsoft.DataTransfer.DocumentDb.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.UnitTests.Sink
{
    sealed class DocumentDbWriteClientMock : IDocumentDbWriteClient
    {
        private HashSet<string> createdCollections;

        private int numberOfDocumentsCreated;
        private int numberOfDocumentsUpserted;

        private HashSet<string> createdStoredProcedures;
        private HashSet<string> deletedStoredProcedures;

        public ICollection<string> CreatedCollections { get { return createdCollections; } }

        public int NumberOfDocumentsCreated { get { return numberOfDocumentsCreated; } }
        public int NumberOfDocumentsUpserted { get { return numberOfDocumentsUpserted; } }

        public ICollection<string> CreatedStoredProcedures { get { return createdStoredProcedures; } }
        public ICollection<string> DeletedStoredProcedures { get { return deletedStoredProcedures; } }

        public DocumentDbWriteClientMock()
        {
            createdCollections = new HashSet<string>();
            createdStoredProcedures = new HashSet<string>();
            deletedStoredProcedures = new HashSet<string>();
        }

        public Task<string> GetOrCreateCollectionAsync(
            string collectionName, string partitionKey, int desiredThroughput, IndexingPolicy indexingPolicy, CancellationToken cancellation)
        {
            Assert.IsFalse(String.IsNullOrEmpty(collectionName), TestResources.MissingCollectionNameInGetOrCreateCollection);

            createdCollections.Add(collectionName);
            return Task.FromResult(collectionName);
        }

        public Task CreateDocumentAsync(string collectionLink, object document, bool disableAutomaticIdGeneration)
        {
            Assert.IsFalse(String.IsNullOrEmpty(collectionLink), TestResources.MissingCollectionLinkInCreateDocumentAsync);
            Assert.IsNotNull(document, TestResources.MissingDocumentInCreateDocumentAsync);

            Interlocked.Increment(ref numberOfDocumentsCreated);
            return TaskHelper.NoOp;
        }

        public Task UpsertDocumentAsync(string collectionLink, object document, bool disableAutomaticIdGeneration)
        {
            Assert.IsFalse(String.IsNullOrEmpty(collectionLink), TestResources.MissingCollectionLinkInUpsertDocumentAsync);
            Assert.IsNotNull(document, TestResources.MissingDocumentInUpsertDocumentAsync);

            Interlocked.Increment(ref numberOfDocumentsUpserted);
            return TaskHelper.NoOp;
        }

        public Task<string> CreateStoredProcedureAsync(string collectionLink, string name, string body)
        {
            createdStoredProcedures.Add(name);
            return Task.FromResult(name);
        }

        public Task<StoredProcedureResult<TResult>> ExecuteStoredProcedureAsync<TResult>(string storedProcedureLink, params dynamic[] args)
        {
            throw new NotSupportedException();
        }

        public Task DeleteStoredProcedureAsync(string storedProcedureLink)
        {
            deletedStoredProcedures.Add(storedProcedureLink);
            return TaskHelper.NoOp;
        }

        public void Dispose() { }
    }
}
