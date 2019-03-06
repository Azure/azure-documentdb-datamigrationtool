using Microsoft.Azure.CosmosDB.Table;

namespace Microsoft.DataTransfer.Core.RemoteLogging
{
    /// <summary>
    /// Class to define the schema for Logging data being sent to the table
    /// </summary>
    public class LoggingTableEntity : TableEntity
    {
        /// <summary>
        /// Exception details
        /// </summary>
        public string FailureException { get; set; }

        /// <summary>
        /// Machine name where the failure occurred
        /// </summary>
        public string FailureMachineName { get; set; }

        /// <summary>
        /// Any additional details (example: all row keys in a batch operation)
        /// </summary>
        public string AdditionalDetails { get; set; }

        /// <summary>
        /// Initializes a new instance of the LoggingTableEntity class.
        /// </summary>
        /// <param name="partitionKey">Partition key value</param>
        /// <param name="rowKey">Row key value</param>
        /// <param name="exception">Exception details</param>
        /// <param name="machineName">Machine name where the failure occurred</param>
        /// <param name="additionalDetails">Any additional details (example: all row keys in a batch operation)</param>
        public LoggingTableEntity(string partitionKey, string rowKey, string exception, string machineName, string additionalDetails)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            FailureException = exception;
            FailureMachineName = machineName;
            AdditionalDetails = additionalDetails;
        }
    }
}
