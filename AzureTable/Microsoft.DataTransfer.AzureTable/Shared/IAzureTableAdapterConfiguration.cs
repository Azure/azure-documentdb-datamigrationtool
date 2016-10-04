using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.AzureTable.Shared
{
    /// <summary>
    /// Contains basic configuration for Azure Table storage data adapters.
    /// </summary>
    public interface IAzureTableAdapterConfiguration
    {
        /// <summary>
        /// Gets the connection string for the Azure Table storage.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "ConnectionString")]
        string ConnectionString { get; }

        /// <summary>
        /// Gets the location mode to use when connecting to Azure Storage.
        /// </summary>
        [Display(ResourceType = typeof(DynamicConfigurationResources), Description = "LocationMode")]
        AzureStorageLocationMode? LocationMode { get; }
    }
}
