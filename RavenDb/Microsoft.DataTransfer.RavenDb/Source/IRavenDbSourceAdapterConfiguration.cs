using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.DataTransfer.RavenDb.Shared;

namespace Microsoft.DataTransfer.RavenDb.Source
{
    /// <summary>
    /// Contains configuration for CSV files data source.
    /// </summary>
    public interface IRavenDbSourceAdapterConfiguration : IRavenDbDataAdapterConfiguration
    {
        /// <summary>
        /// Gets the list of file names to read CSV data from.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_Collections")]
        IEnumerable<string> Collections { get; }
    }
}
