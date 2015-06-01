using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.RavenDb.Shared
{
    /// <summary>
    /// Contains basic configuration for RavenDB data adapters.
    /// </summary>
    public interface IRavenDbAdapterConfiguration
    {
        /// <summary>
        /// Gets the connection string for the RavenDB database.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "ConnectionString")]
        string ConnectionString { get; }
    }
}
