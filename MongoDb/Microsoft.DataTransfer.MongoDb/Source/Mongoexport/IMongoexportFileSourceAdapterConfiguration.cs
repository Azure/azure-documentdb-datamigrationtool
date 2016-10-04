using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.MongoDb.Source.Mongoexport
{
    /// <summary>
    /// Contains configuration for mongoexport JSON files data adapter.
    /// </summary>
    public interface IMongoexportFileSourceAdapterConfiguration
    {
        /// <summary>
        /// Gets the list of file names to read mongoexport JSON data from.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "MongoexportSource_Files")]
        IEnumerable<string> Files { get; }

        /// <summary>
        /// Gets the value that indicates whether input files should be decompressed.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "MongoexportSource_Decompress")]
        bool Decompress { get; }
    }
}
