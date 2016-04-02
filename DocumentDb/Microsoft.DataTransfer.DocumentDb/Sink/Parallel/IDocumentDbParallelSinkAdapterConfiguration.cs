using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Parallel
{
    /// <summary>
    /// Contains configuration for parallel DocumentDB data sink adapter.
    /// </summary>
    public interface IDocumentDbParallelSinkAdapterConfiguration : IDocumentDbSinkAdapterConfiguration
    {
        /// <summary>
        /// Gets the documents collection name.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "ParallelSink_Collection")]
        string Collection { get; }

        /// <summary>
        /// Gets the collection throughput for V2 collections.
        /// </summary>
        [Display(ResourceType = typeof(DynamicConfigurationResources), Description = "ParallelSink_CollectionThroughput")]
        int? CollectionThroughput { get; }

        /// <summary>
        /// Gets the path to the property used as partition key.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "ParallelSink_PartitionKey")]
        string PartitionKey { get; }

        /// <summary>
        /// Gets the maximum number of parallel insert operations.
        /// </summary>
        [Display(ResourceType = typeof(DynamicConfigurationResources), Description = "ParallelSink_ParallelRequests")]
        int? ParallelRequests { get; }
    }
}
