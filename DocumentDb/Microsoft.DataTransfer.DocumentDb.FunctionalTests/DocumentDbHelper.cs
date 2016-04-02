using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Client.TransientFaultHandling;
using Microsoft.DataTransfer.DocumentDb.Client;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.FunctionalTests
{
    static class DocumentDbHelper
    {
        public static async Task CreateSampleCollectionAsync(string connectionString, string collectionName, IEnumerable<object> documents)
        {
            var connectionSettings = DocumentDbConnectionStringBuilder.Parse(connectionString);

            using (var client = CreateClient(connectionSettings))
            {
                var database = await client.CreateDatabaseAsync(new Database { Id = connectionSettings.Database });
                var collection = await client.CreateDocumentCollectionAsync(database.Resource.SelfLink, new DocumentCollection { Id = collectionName });

                foreach (var document in documents)
                    await client.CreateDocumentAsync(collection.Resource.SelfLink, document);
            }
        }

        public static IEnumerable<IReadOnlyDictionary<string, object>> ReadDocuments(string connectionString, string collectionName,
            string query = "SELECT * FROM c")
        {
            var connectionSettings = DocumentDbConnectionStringBuilder.Parse(connectionString);

            using (var client = CreateClient(connectionSettings))
            {
                var database = client
                    .CreateDatabaseQuery()
                    .Where(d => d.Id == connectionSettings.Database)
                    .AsEnumerable()
                    .FirstOrDefault();

                Assert.IsNotNull(database, "Document database does not exist.");

                var collection = client
                    .CreateDocumentCollectionQuery(database.CollectionsLink)
                    .Where(c => c.Id == collectionName)
                    .AsEnumerable()
                    .FirstOrDefault();

                Assert.IsNotNull(collection, "Document collection does not exist.");

                return client
                    .CreateDocumentQuery<Dictionary<string, object>>(collection.DocumentsLink, query, new FeedOptions { EnableCrossPartitionQuery = true })
                    .ToArray();
            }
        }

        public static async Task DeleteDatabaseAsync(string connectionString)
        {
            var connectionSettings = DocumentDbConnectionStringBuilder.Parse(connectionString);

            using (var client = CreateClient(connectionSettings))
            {
                var database = client
                    .CreateDatabaseQuery()
                    .Where(d => d.Id == connectionSettings.Database)
                    .AsEnumerable()
                    .FirstOrDefault();

                if (database != null)
                    await client.DeleteDatabaseAsync(database.SelfLink);
            }
        }

        private static IReliableReadWriteDocumentClient CreateClient(IDocumentDbConnectionSettings connectionSettings)
        {
            return new DocumentClient(new Uri(connectionSettings.AccountEndpoint), connectionSettings.AccountKey)
                .AsReliable(new FixedInterval(10, TimeSpan.FromSeconds(1)));
        }
    }
}
