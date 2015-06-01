using Microsoft.DataTransfer.RavenDb.Shared;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.RavenDb.Source
{
    /// <summary>
    /// Contains configuration for RavenDB data source adapter.
    /// </summary>
    public interface IRavenDbSourceAdapterConfiguration : IRavenDbAdapterConfiguration
    {
        /// <summary>
        /// Gets the name of the index to use for query.
        /// </summary>
        [Display(ResourceType = typeof(DynamicConfigurationResources), Description = "Source_Index")]
        string Index { get; }

        /// <summary>
        /// Gets the Lucene query.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_Query")]
        string Query { get; }

        /// <summary>
        /// Gets the path to the file that contains Lucene query.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_QueryFile")]
        string QueryFile { get; }

        /// <summary>
        /// Gets the value that indicates whether document id should be omitted in the output. 
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_ExcludeId")]
        bool ExcludeId { get; }
    }
}
