using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CosmosDB;
using Microsoft.Azure.CosmosDB.Table;
using Microsoft.Azure.Storage;

namespace Microsoft.DataTransfer.Core.RemoteLogging
{
    /// <summary>
    /// Class to support remote logging in CosmosDB Tables
    /// </summary>
    public sealed class RemoteLogging : IRemoteLogging
    {
        private static CloudTable migrationLogger;
        private readonly int tableThroughput = 20000;
        private readonly string tableName = "DtErrorLogs";

        /// <summary>
        /// Initializes a new instance of the RemoteLogging class.
        /// </summary>
        /// <param name="storageAccount">storage account object</param>
        /// <param name="connectionPolicy">additional connection policies</param>
        public RemoteLogging(CloudStorageAccount storageAccount, TableConnectionPolicy connectionPolicy)
        {
            CloudTableClient tcMigrationLogger = storageAccount.CreateCloudTableClient(connectionPolicy: connectionPolicy);
            migrationLogger = tcMigrationLogger.GetTableReference(tableName);
        }

        /// <summary>
        /// Create a Table if it does not exist already
        /// </summary>
        public async Task<bool> CreateRemoteLoggingTableIfNotExists(CancellationToken cancellation)
        {
            return await migrationLogger.CreateIfNotExistsAsync(IndexingMode.Consistent, tableThroughput, cancellation);
        }

        /// <summary>
        /// Log the failures that occurred as a result of using DT. Given this is logging code, it will not throw errors to prevent
        /// crashing the application
        /// </summary>
        /// <param name="partitionKey">partition key for the remote table</param>
        /// <param name="rowKeys">rowKey for the remote table</param>
        /// <param name="exception">failure details</param>
        /// <param name="additionalDetails">any additional details</param>
        public async void LogFailures(string partitionKey, string rowKeys, string exception, string additionalDetails = null)
        {
            try
            {
                // Log the failure to a cosmos db table in the provided account
                LoggingTableEntity log = new LoggingTableEntity(partitionKey, rowKeys,
                    exception, Environment.MachineName, additionalDetails);
                TableOperation loggingOp = TableOperation.InsertOrReplace(log);
                TableResult result = await migrationLogger.ExecuteAsync(loggingOp);
            }
            catch (Exception excp)
            {
                Console.WriteLine("Failed to log to the remote table. " + excp.Message);
            }
        }
    }
}
