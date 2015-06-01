using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Client.TransientFaultHandling;
using Microsoft.Azure.Documents.Linq;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.DocumentDb.Client.Enumeration;
using Microsoft.DataTransfer.DocumentDb.Client.PartitionResolvers;
using Microsoft.DataTransfer.DocumentDb.Client.Serialization;
using Microsoft.DataTransfer.DocumentDb.Sink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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

        public async Task<string> GetOrCreateCollectionAsync(string collectionName, CollectionPricingTier collectionTier)
        {
            Guard.NotEmpty("collectionName", collectionName);

            var database = await GetOrCreateDatabase(databaseName);
            var collection = await TryGetCollection(database, collectionName);

            if (collection == null)
            {
                var alreadyExists = false;

                try
                {
                    collection = await client.CreateDocumentCollectionAsync(database.SelfLink,
                        new DocumentCollection { Id = collectionName }, new RequestOptions { OfferType = ToOfferType(collectionTier) });
                }
                catch (DocumentClientException clientException)
                {
                    if (clientException.StatusCode != HttpStatusCode.Conflict)
                        throw;

                    alreadyExists = true;
                }

                if (alreadyExists)
                    collection = await TryGetCollection(database, collectionName);
            }

            if (collection == null)
                throw Errors.FailedToCreateCollection(collectionName);

            return collection.SelfLink;
        }

        private static string ToOfferType(CollectionPricingTier collectionTier)
        {
            return Enum.GetName(typeof(CollectionPricingTier), collectionTier).ToUpperInvariant();
        }

        private Task<DocumentCollection> TryGetCollection(Database database, string collectionName)
        {
            return client
                .CreateDocumentCollectionQuery(database.CollectionsLink)
                .Where(c => c.Id == collectionName)
                .AsDocumentQuery()
                .FirstOrDefault();
        }

        public Task CreateDocumentAsync(string collectionLink, object document, bool disableAutomaticIdGeneration)
        {
            Guard.NotEmpty("collectionLink", collectionLink);
            Guard.NotNull("document", document);

            return client.CreateDocumentAsync(collectionLink, document, null, disableAutomaticIdGeneration);
        }

        public async Task<string> CreateStoredProcedureAsync(string collectionLink, string name, string body)
        {
            var response = await client.CreateStoredProcedureAsync(collectionLink, new StoredProcedure { Id = name, Body = body });
            if (response == null || response.Resource == null)
                throw Errors.FailedToCreateStoredProcedure(name);

            return response.Resource.SelfLink;
        }

        public async Task<TResult> ExecuteStoredProcedureAsync<TResult>(string storedProcedureLink, params dynamic[] args)
        {
            return (await client.ExecuteStoredProcedureAsync<TResult>(storedProcedureLink, args)).Response;
        }

        public Task DeleteStoredProcedureAsync(string storedProcedureLink)
        {
            return client.DeleteStoredProcedureAsync(storedProcedureLink);
        }

        public async Task<IAsyncEnumerator<IReadOnlyDictionary<string, object>>> QueryDocumentsAsync(string collectionNamePattern, string query)
        {
            Guard.NotEmpty("collectionNamePattern", collectionNamePattern);

            var database = await TryGetDatabase(databaseName);
            if (database == null)
                return EmptyAsyncEnumerator<IReadOnlyDictionary<string, object>>.Instance;

            var matchingCollections = await GetMatchingCollections(database, collectionNamePattern);
            if (matchingCollections == null || !matchingCollections.Any())
                return EmptyAsyncEnumerator<IReadOnlyDictionary<string, object>>.Instance;

            // Use SDK to query multiple collections, client will not be thread-safe
            client.UnderlyingClient.PartitionResolvers[database.SelfLink] = new FairPartitionResolver(matchingCollections);

            var documentQuery = 
                String.IsNullOrEmpty(query)
                    ? client.CreateDocumentQuery<DocumentSurrogate>(database.SelfLink)
                    : client.CreateDocumentQuery<DocumentSurrogate>(database.SelfLink, query);

            return new DocumentSurrogateQueryAsyncEnumerator(documentQuery.AsDocumentQuery());
        }

        private async Task<IReadOnlyList<string>> GetMatchingCollections(Database database, string collectionNamePattern)
        {
            var result = new List<string>();

            using (var enumerator = new AsyncEnumerator<DocumentCollection>(
                client.CreateDocumentCollectionQuery(database.CollectionsLink).AsDocumentQuery()))
            {
                while (await enumerator.MoveNextAsync())
                    if (Regex.IsMatch(enumerator.Current.Id, collectionNamePattern))
                        result.Add(enumerator.Current.SelfLink);
            }

            return result;
        }

        private async Task<Database> GetOrCreateDatabase(string name)
        {
            Guard.NotEmpty("name", name);

            var database = await TryGetDatabase(name);

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
                    database = await TryGetDatabase(name);
            }

            if (database == null)
                throw Errors.FailedToCreateDatabase(name);

            return database;
        }

        private Task<Database> TryGetDatabase(string name)
        {
            return client
                .CreateDatabaseQuery()
                .Where(d => d.Id == name)
                .AsDocumentQuery()
                .FirstOrDefault();
        }

        public void Dispose()
        {
            TrashCan.Throw(ref client);
        }
    }
}
