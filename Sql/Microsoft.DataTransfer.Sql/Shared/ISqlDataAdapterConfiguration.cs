using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.Sql.Shared
{
    /// <summary>
    /// Contains basic configuration for SQL data adapters.
    /// </summary>
    public interface ISqlDataAdapterConfiguration
    {
        /// <summary>
        /// Gets the connection string for the SQL database.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "ConnectionString")]
        string ConnectionString { get; }
    }
}
