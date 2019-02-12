using Microsoft.Azure.CosmosDB.Table;

namespace Microsoft.DataTransfer.AzureTable.RemoteLogging
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
        /// Constructor to initialize logging table entity values
        /// </summary>
        /// <param name="partitionKey"></param>
        /// <param name="rowKey"></param>
        /// <param name="exception"></param>
        /// <param name="machineName"></param>
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
