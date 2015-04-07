using Microsoft.DataTransfer.Sql.Shared;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.Sql.Source
{
    /// <summary>
    /// Contains configuration for SQL data source adapter.
    /// </summary>
    public interface ISqlDataSourceAdapterConfiguration : ISqlDataAdapterConfiguration
    {
        /// <summary>
        /// Gets the SQL query.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_Query")]
        string Query { get; }

        /// <summary>
        /// Gets the path to the file that contains SQL query document.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_QueryFile")]
        string QueryFile { get; }

        /// <summary>
        /// Gets the separator sequence to identify nested documents from column names.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_NestingSeparator")]
        string NestingSeparator { get; }
    }
}
