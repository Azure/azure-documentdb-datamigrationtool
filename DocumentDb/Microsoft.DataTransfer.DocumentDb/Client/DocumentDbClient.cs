using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Client.TransientFaultHandling;
using Microsoft.Azure.Documents.Linq;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.DocumentDb.Client.Enumeration;
using Microsoft.DataTransfer.DocumentDb.Client.PartitionResolvers;
using Microsoft.DataTransfer.DocumentDb.Client.Serialization;
using Microsoft.DataTransfer.Extensibility.Basics.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Client
{
    sealed class DocumentDbClient : IDocumentDbWriteClient, IDocumentDbReadClient
    {
        private IReliableReadWriteDocumentClient client;
        private string databaseName;

        public DocumentDbClient(IReliableReadWriteDocumentClient client, string databaseName)
        {
            Guard.NotNull("client", client);
            Guard.NotEmpty("databaseName", databaseName);

            this.client = client;
            this.databaseName = databaseName;
        }

        public Task<string> GetOrCreateCollectionAsync(
            string collectionName, string partitionKey, int desiredThroughput, IndexingPolicy indexingPolicy, CancellationToken cancellation)
        {
            return GetOrCreateCollectionAsyncInternal(collectionName, partitionKey,
                new RequestOptions { OfferThroughput = desiredThroughput }, indexingPolicy, cancellation);
        }

        public async Task<string> GetOrCreateCollectionAsyncInternal(
            string collectionName, string partitionKey, RequestOptions requestOptions, IndexingPolicy indexingPolicy, CancellationToken cancellation)
        {
            Guard.NotEmpty("collectionName", collectionName);

            var database = await GetOrCreateDatabase(databaseName, cancellation);
            var collection = await TryGetCollection(database, collectionName, cancellation);

            if (collection == null)
            {
                var alreadyExists = false;

                try
                {
                    var collectionDefinition = new DocumentCollection { Id = collectionName };

                    if (!String.IsNullOrEmpty(partitionKey))
                        collectionDefinition.PartitionKey.Paths.Add(partitionKey);

                    if (indexingPolicy != null)
                        collectionDefinition.IndexingPolicy = indexingPolicy;

                    collection = await client.CreateDocumentCollectionAsync(
                        database.SelfLink, collectionDefinition, requestOptions);
                }
                catch (DocumentClientException clientException)
                {
                    if (clientException.StatusCode != HttpStatusCode.Conflict)
                        throw;

                    alreadyExists = true;
                }

                if (alreadyExists)
                    collection = await TryGetCollection(database, collectionName, cancellation);
            }

            if (collection == null)
                throw Errors.FailedToCreateCollection(collectionName);

            return collection.SelfLink;
        }

        private Task<DocumentCollection> TryGetCollection(Database database, string collectionName, CancellationToken cancellation)
        {
            return client
                .CreateDocumentCollectionQuery(database.CollectionsLink)
                .Where(c => c.Id == collectionName)
                .AsDocumentQuery()
                .FirstOrDefault(cancellation);
        }

        public Task CreateDocumentAsync(string collectionLink, object document, bool disableAutomaticIdGeneration)
        {
            Guard.NotEmpty("collectionLink", collectionLink);
            Guard.NotNull("document", document);

            return client.CreateDocumentAsync(collectionLink, document,
                disableAutomaticIdGeneration: disableAutomaticIdGeneration);
        }

        public Task UpsertDocumentAsync(string collectionLink, object document, bool disableAutomaticIdGeneration)
        {
            Guard.NotEmpty("collectionLink", collectionLink);
            Guard.NotNull("document", document);

            return client.UpsertDocumentAsync(collectionLink, document,
                disableAutomaticIdGeneration: disableAutomaticIdGeneration);
        }

        public async Task<string> CreateStoredProcedureAsync(string collectionLink, string name, string body)
        {
            var response = await client.CreateStoredProcedureAsync(collectionLink, new StoredProcedure { Id = name, Body = body });
            if (response == null || response.Resource == null)
                throw Errors.FailedToCreateStoredProcedure(name);

            return response.Resource.SelfLink;
        }

        public async Task<StoredProcedureResult<TResult>> ExecuteStoredProcedureAsync<TResult>(string storedProcedureLink, params dynamic[] args)
        {
            var result = await client.ExecuteStoredProcedureAsync<TResult>(storedProcedureLink, args);
            return new StoredProcedureResult<TResult>(result.ActivityId, result.Response);
        }

        public Task DeleteStoredProcedureAsync(string storedProcedureLink)
        {
            return client.DeleteStoredProcedureAsync(storedProcedureLink);
        }

        public async Task<IAsyncEnumerator<IReadOnlyDictionary<string, object>>> QueryDocumentsAsync(string collectionNamePattern, string query, CancellationToken cancellation)
        {
            Guard.NotEmpty("collectionNamePattern", collectionNamePattern);

            var database = await TryGetDatabase(databaseName, cancellation);
            if (database == null)
                return EmptyAsyncEnumerator<IReadOnlyDictionary<string, object>>.Instance;

            var matchingCollections = await GetMatchingCollections(database, collectionNamePattern, cancellation);
            if (matchingCollections == null || !matchingCollections.Any())
                return EmptyAsyncEnumerator<IReadOnlyDictionary<string, object>>.Instance;

            // Use SDK to query multiple collections, client will not be thread-safe
            client.UnderlyingClient.PartitionResolvers[database.SelfLink] = new FairPartitionResolver(matchingCollections);

            var feedOptions = new FeedOptions
            {
                EnableCrossPartitionQuery = true,
                MaxItemCount = -1
            };

            var documentQuery = 
                String.IsNullOrEmpty(query)
                    ? client.CreateDocumentQuery<DocumentSurrogate>(database.SelfLink, feedOptions)
                    : client.CreateDocumentQuery<DocumentSurrogate>(database.SelfLink, query, feedOptions);

            return new DocumentSurrogateQueryAsyncEnumerator(documentQuery.AsDocumentQuery());
        }

        private async Task<IReadOnlyList<string>> GetMatchingCollections(Database database, string collectionNamePattern, CancellationToken cancellation)
        {
            var result = new List<string>();

            using (var enumerator = new AsyncEnumerator<DocumentCollection>(
                client.CreateDocumentCollectionQuery(database.CollectionsLink).AsDocumentQuery()))
            {
                var collectionNameRegex = new Regex(collectionNamePattern, RegexOptions.Compiled);

                while (await enumerator.MoveNextAsync(cancellation))
                {
                    var match = collectionNameRegex.Match(enumerator.Current.Id);
                    // Make sure regex matches entire collection name and not just substring
                    if (match.Success && String.Equals(match.Value, enumerator.Current.Id, StringComparison.InvariantCulture))
                        result.Add(enumerator.Current.SelfLink);
                }
            }

            return result;
        }

        private async Task<Database> GetOrCreateDatabase(string name, CancellationToken cancellation)
        {
            Guard.NotEmpty("name", name);

            var database = await TryGetDatabase(name, cancellation);

            if (database == null)
            {
                var alreadyExists = false;

                try
                {
                    database = await client.CreateDatabaseAsync(new Database { Id = name });
                }
                catch (DocumentClientException clientException)
                {
                    if (clientException.StatusCode != HttpStatusCode.Conflict)
                        throw;

                    alreadyExists = true;
                }

                if (alreadyExists)
                    database = await TryGetDatabase(name, cancellation);
            }

            if (database == null)
                throw Errors.FailedToCreateDatabase(name);

            return database;
        }

        private Task<Database> TryGetDatabase(string name, CancellationToken cancellation)
        {
            return client
                .CreateDatabaseQuery()
                .Where(d => d.Id == name)
                .AsDocumentQuery()
                .FirstOrDefault(cancellation);
        }

        public void Dispose()
        {
            TrashCan.Throw(ref client);
        }
    }
}
