using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.FirebaseJsonFile.Source
{
    /// <summary>
    /// Contains configuration for Firebase JSON files data source adapter. 
    /// </summary>
    public interface IFirebaseJsonFileSourceAdapterConfiguration
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

        /// <summary>
        /// Gets an optional value indicating the node path to import from the source files
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_Node")]
        string Node { get; }

        /// <summary>
        /// Gets the field name to be used for storing the Firebase ID
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_IdField")]
        string IdField { get; }

        /// <summary>
        /// Gets the field name to be used for storing the Firebase top-level node name (to be interpreted as a collection)
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_CollectionField")]
        string CollectionField { get; }
    }
}
