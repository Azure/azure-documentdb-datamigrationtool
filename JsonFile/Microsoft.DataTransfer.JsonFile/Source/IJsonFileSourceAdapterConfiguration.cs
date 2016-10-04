using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.JsonFile.Source
{
    /// <summary>
    /// Contains configuration for JSON files data source adapter. 
    /// </summary>
    public interface IJsonFileSourceAdapterConfiguration
    {
        /// <summary>
        /// Gets the list of file names to read JSON data from.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_Files")]
        IEnumerable<string> Files { get; }

        /// <summary>
        /// Gets the value that indicates whether input files should be decompressed.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_Decompress")]
        bool Decompress { get; }
    }
}
