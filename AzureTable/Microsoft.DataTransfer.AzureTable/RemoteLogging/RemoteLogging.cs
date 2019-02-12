using System;
using System.Threading;
using Microsoft.Azure.CosmosDB;
using Microsoft.Azure.CosmosDB.Table;
using Microsoft.Azure.Storage;

namespace Microsoft.DataTransfer.AzureTable.RemoteLogging
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RemoteLogging : IRemoteLogging
    {
        private static CloudTable migrationLogger;
        private readonly int tableThroughput = 20000;
        private readonly string tableName = "migrationLogs";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storageAccount"></param>
        /// <param name="connectionPolicy"></param>
        public RemoteLogging(CloudStorageAccount storageAccount, TableConnectionPolicy connectionPolicy)
        {
            CloudTableClient tcMigrationLogger = storageAccount.CreateCloudTableClient(connectionPolicy: connectionPolicy);
            migrationLogger = tcMigrationLogger.GetTableReference(tableName);
        }

        /// <summary>
        /// 
        /// </summary>
        public async void CreateRemoteLoggingTable(CancellationToken cancellation)
        {
            await migrationLogger.CreateIfNotExistsAsync(IndexingMode.Consistent, tableThroughput, cancellation);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="partitionKey"></param>
        /// <param name="rowKeys"></param>
        /// <param name="exception"></param>
        /// <param name="additionalDetails"></param>
        public async void LogFailures(string partitionKey, string rowKeys, string exception, string additionalDetails = null)
        {
            //Log the failure to a cosmosDB table in  the provided account.
            LoggingTableEntity log = new LoggingTableEntity(partitionKey, rowKeys,
                exception, Environment.MachineName, additionalDetails);
            TableOperation loggingOp = TableOperation.InsertOrReplace(log);
            TableResult result = await migrationLogger.ExecuteAsync(loggingOp);
        }
    }
}
