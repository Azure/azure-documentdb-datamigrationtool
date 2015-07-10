using Microsoft.DataTransfer.HBase.Shared;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.HBase.Source
{
    /// <summary>
    /// Contains configuration for HBase data source adapter.
    /// </summary>
    public interface IHBaseSourceAdapterConfiguration : IHBaseAdapterConfiguration
    {
        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_Table")]
        string Table { get; }

        /// <summary>
        /// Gets the scan filter.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_Filter")]
        string Filter { get; }

        /// <summary>
        /// Gets the path to the file that contains scan filter.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_FilterFile")]
        string FilterFile { get; }

        /// <summary>
        /// Gets the value that indicates whether row id should be omitted in the output. 
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_ExcludeId")]
        bool ExcludeId { get; }

        /// <summary>
        /// Gets the size of the batch.
        /// </summary>
        [Display(ResourceType = typeof(DynamicConfigurationResources), Description = "Source_BatchSize")]
        int? BatchSize { get; }
    }
}
