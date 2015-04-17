using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.RavenDb.Shared
{
    /// <summary>
    /// Contains basic configuration for SQL data adapters.
    /// </summary>
    public interface IRavenDbDataAdapterConfiguration
    {
        /// <summary>
        /// Gets the connection string for the SQL database.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "ConnectionString")]
        string ConnectionString { get; }
    }
}
