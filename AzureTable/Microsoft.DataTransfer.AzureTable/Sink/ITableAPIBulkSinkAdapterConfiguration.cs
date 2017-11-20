using Microsoft.DataTransfer.AzureTable;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.TableAPI.Sink.Bulk
{
    /// <summary>
    /// Contains configuration for Azure CosmosDB Table data bulk sink adapter. 
    /// </summary>
    public interface ITableAPIBulkSinkAdapterConfiguration
    {
        /// <summary>
        /// Gets the connection string of the Azure CosmosDB Table to connect to.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "ConnectionString")]
        string ConnectionString { get; }

        /// <summary>
        /// Gets the name of the Azure CosmosDB Table to store data to.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "BulkSink_TableName")]
        string TableName { get; }

        /// <summary>
        /// Gets the value that indicates whether target data can be overwritten.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "BulkSink_Overwrite")]
        bool Overwrite { get; }

        /// <summary>
        /// Gets the size of the data to accumulate in memory before committing a batch.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "BulkSink_MaxInputBufferSize")]
        int? MaxInputBufferSize { get; }

        /// <summary>
        /// Gets the configured throughput in RU/s.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "BulkSink_Throughput")]
        int? Throughput { get; }

        /// <summary>
        /// Gets the size of data to be bundled into one batch for commit.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "BulkSink_MaxBatchSize")]
        int? MaxBatchSize { get; }
    }
}
