using Microsoft.Azure.CosmosDB.Table;
using Microsoft.Azure.Storage;
using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.AzureTable.FunctionalTests
{
    static class AzureTableHelper
    {
        public static void CreateTable(string connectionString, string tableName, IReadOnlyDictionary<string, object>[] data)
        {
            var table = GetTable(connectionString, tableName);
            table.CreateIfNotExists();

            TableBatchOperation batch = new TableBatchOperation();
            foreach (var entity in data)
            {
                batch.Insert(new DictionaryTableEntity(Guid.NewGuid().ToString(), entity));

                if (batch.Count >= 100)
                {
                    table.ExecuteBatch(batch);
                    batch.Clear();
                }
            }

            if (batch.Count > 0)
                table.ExecuteBatch(batch);
        }

        public static void DeleteTable(string connectionString, string tableName)
        {
            GetTable(connectionString, tableName).DeleteIfExists();
        }

        private static CloudTable GetTable(string connectionString, string tableName)
        {
            return CloudStorageAccount
                .Parse(connectionString)
                .CreateCloudTableClient()
                .GetTableReference(tableName);
        }
    }
}
