using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.CsvFile.Source
{
    /// <summary>
    /// Contains configuration for CSV files data source.
    /// </summary>
    public interface ICsvFileSourceAdapterConfiguration
    {
        /// <summary>
        /// Gets the list of file names to read CSV data from.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_Files")]
        IEnumerable<string> Files { get; }

        /// <summary>
        /// Gets the separator sequence to identify nested documents from column names.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_NestingSeparator")]
        string NestingSeparator { get; }

        /// <summary>
        /// Gets the value that indicates whether empty space in the quoted values should be trimmed.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_TrimQuoted")]
        bool TrimQuoted { get; }

        /// <summary>
        /// Gets the value that indicates whether unquoted NULL string should be treated as string.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_NoUnquotedNulls")]
        bool NoUnquotedNulls { get; }

        /// <summary>
        /// Gets the value that indicates whether regional format settings should be used to parse the input files.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_UseRegionalSettings")]
        bool UseRegionalSettings { get; }

        /// <summary>
        /// Gets the value that indicates whether input files should be decompressed.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_Decompress")]
        bool Decompress { get; }
    }
}
