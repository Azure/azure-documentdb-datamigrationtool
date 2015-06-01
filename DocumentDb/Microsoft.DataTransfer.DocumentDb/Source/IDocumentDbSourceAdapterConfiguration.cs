using Microsoft.DataTransfer.DocumentDb.Shared;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.DocumentDb.Source
{
    /// <summary>
    /// Contains configuration for DocumentDB data source adapter.
    /// </summary>
    public interface IDocumentDbSourceAdapterConfiguration : IDocumentDbAdapterConfiguration
    {
        /// <summary>
        /// Gets the documents collection name pattern.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_Collection")]
        string Collection { get; }

        /// <summary>
        /// Gets the value that indicates whether internal DocumentDB fields should be emitted in the output.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_InternalFields")]
        bool InternalFields { get; }

        /// <summary>
        /// Gets the documents query.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_Query")]
        string Query { get; }

        /// <summary>
        /// Gets the path to the file that contains documents query.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_QueryFile")]
        string QueryFile { get; }
    }
}
