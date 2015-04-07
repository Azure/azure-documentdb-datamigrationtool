using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    /// <summary>
    /// Contains configuration for bulk DocumentDB data sink adapter.
    /// </summary>
    public interface IDocumentDbBulkSinkAdapterConfiguration : IDocumentDbSinkAdapterConfiguration
    {
        /// <summary>
        /// Gets the bulk import stored procedure file name.
        /// </summary>
        [Display(ResourceType = typeof(DynamicConfigurationResources), Description = "BulkSink_StoredProcFile")]
        string StoredProcFile { get; }

        /// <summary>
        /// Gets the number of data artifacts bundled into one batch.
        /// </summary>
        [Display(ResourceType = typeof(DynamicConfigurationResources), Description = "BulkSink_BatchSize")]
        int? BatchSize { get; }

        /// <summary>
        /// Gets the maximum size of the JSON payload that can be sent to the server.
        /// </summary>
        [Display(ResourceType = typeof(DynamicConfigurationResources), Description = "BulkSink_MaxScriptSize")]
        int? MaxScriptSize { get; }
    }
}
