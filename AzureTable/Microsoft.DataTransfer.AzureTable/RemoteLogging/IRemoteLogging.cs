using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.AzureTable
{
    /// <summary>
    /// The interface for remotely logging to Cosmos tables, when it is used as a sink endpoint.
    /// This is beneficial to consolidate failure logs when multiple instances of data migration
    /// tool are run simultaneously. 
    /// </summary>
    public interface IRemoteLogging
    {
        /// <summary>
        /// Log failures that occurred during a data migration.
        /// </summary>
        /// <param name="partitionKey"></param>
        /// <param name="rowKeys"></param>
        /// <param name="exception"></param>
        /// <param name="additionalDetails"></param>
        void LogFailures(string partitionKey, string rowKeys, string exception, string additionalDetails = null);
        
        /// <summary>
        /// Create a CosmosDB table for failure logs
        /// </summary>
        /// <param name="cancellation"></param>
        Task<bool> CreateRemoteLoggingTableIfNotExists(CancellationToken cancellation);
    }
}
