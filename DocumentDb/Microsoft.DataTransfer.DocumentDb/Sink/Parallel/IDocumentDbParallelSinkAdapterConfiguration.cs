using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Parallel
{
    /// <summary>
    /// Contains configuration for parallel DocumentDB data sink adapter.
    /// </summary>
    public interface IDocumentDbParallelSinkAdapterConfiguration : IDocumentDbSinkAdapterConfiguration
    {
        /// <summary>
        /// Gets the maximum number of parallel insert operations.
        /// </summary>
        [Display(ResourceType = typeof(DynamicConfigurationResources), Description = "ParallelSink_ParallelRequests")]
        int? ParallelRequests { get; }
    }
}
