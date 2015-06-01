using Raven.Abstractions.Indexing;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.RavenDb.FunctionalTests
{
    static class RavenDbHelper
    {
        public static void CreateSampleDatabase(string connectionString, IEnumerable<object> documents)
        {
            using (var store = CreateStore(connectionString, true))
            {
                using (var bulkInsert = store.BulkInsert())
                {
                    foreach (var document in documents)
                        bulkInsert.Store(document, Guid.NewGuid().ToString());
                }
            }
        }

        public static void CreateIndex(string connectionString, string indexName, string map)
        {
            using (var store = CreateStore(connectionString, false))
            {
                store.DatabaseCommands.PutIndex(indexName, new IndexDefinition { Map = map });
            }
        }

        public static void DeleteDatabase(string connectionString)
        {
            using (var store = CreateStore(connectionString, false))
            {
                store.DatabaseCommands.GlobalAdmin.DeleteDatabase(store.DefaultDatabase, true);
            }
        }

        private static DocumentStore CreateStore(string connectionString, bool createDatabase)
        {
            var store = new DocumentStore();
            store.ParseConnectionString(connectionString);
            store.Initialize(createDatabase);
            return store;
        }
    }
}
