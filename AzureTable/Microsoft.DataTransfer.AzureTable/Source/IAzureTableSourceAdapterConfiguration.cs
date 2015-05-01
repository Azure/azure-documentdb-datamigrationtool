using Microsoft.DataTransfer.AzureTable.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.AzureTable.Source
{
    /// <summary>
    /// Contains configuration for Azure Table storage data source adapter.
    /// </summary>
    public interface IAzureTableSourceAdapterConfiguration : IAzureTableAdapterConfiguration
    {
        /// <summary>
        /// Gets the source name.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_Table")]
        string Table { get; }

        /// <summary>
        /// Gets the value that indicates which internal Azure Table fields should be preserved in the output.
        /// </summary>
        [Display(ResourceType = typeof(DynamicConfigurationResources), Description = "Source_InternalFields")]
        AzureTableInternalFields? InternalFields { get; }

        /// <summary>
        /// Gets the filter string.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_Filter")]
        string Filter { get; }

        /// <summary>
        /// Gets the names of the columns to select.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_Projection")]
        IEnumerable<string> Projection { get; }
    }
}
