using Microsoft.DataTransfer.DocumentDb.Shared;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.DocumentDb.Sink
{
    /// <summary>
    /// Contains basic configuration for DocumentDB data sink adapters.
    /// </summary>
    public interface IDocumentDbSinkAdapterConfiguration : IDocumentDbAdapterConfiguration
    {
        /// <summary>
        /// Gets the collection throughput.
        /// </summary>
        [Display(ResourceType = typeof(DynamicConfigurationResources), Description = "Sink_CollectionThroughput")]
        int? CollectionThroughput { get; }

        /// <summary>
        /// Gets the document collection indexing policy.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Sink_IndexingPolicy")]
        string IndexingPolicy { get; }

        /// <summary>
        /// Gets the path to the file that contains collection indexing policy.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Sink_IndexingPolicyFile")]
        string IndexingPolicyFile { get; }

        /// <summary>
        /// Gets the name of source field that should be treated as document id.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Sink_IdField")]
        string IdField { get; }

        /// <summary>
        /// Gets the value that indicates whether document ids should not be created automatically.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Sink_DisableIdGeneration")]
        bool DisableIdGeneration { get; }

        /// <summary>
        /// Gets the value that indicates whether documents with the same id should be updated.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Sink_UpdateExisting")]
        bool UpdateExisting { get; }

        /// <summary>
        /// Gets the DocumentDB date and time handling strategy.
        /// </summary>
        [Display(ResourceType = typeof(DynamicConfigurationResources), Description = "Sink_Dates")]
        DateTimeHandling? Dates { get; }
    }
}
