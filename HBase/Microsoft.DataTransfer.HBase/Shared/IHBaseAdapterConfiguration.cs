using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.HBase.Shared
{
    /// <summary>
    /// Contains basic configuration for HBase data adapters.
    /// </summary>
    public interface IHBaseAdapterConfiguration
    {
        /// <summary>
        /// Gets the connection string for the HBase Stargate service.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "ConnectionString")]
        string ConnectionString { get; }
    }
}
