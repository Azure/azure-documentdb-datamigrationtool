using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.JsonFile.Sink
{
    /// <summary>
    /// Contains configuration for JSON files data sink adapter. 
    /// </summary>
    public interface IJsonFileSinkAdapterConfiguration
    {
        /// <summary>
        /// Gets the path to the file to store JSON data to.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Sink_File")]
        string File { get; }

        /// <summary>
        /// Gets the value that indicates whether JSON in the target file should be decorated with new lines and indentation.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Sink_Prettify")]
        bool Prettify { get; }

        /// <summary>
        /// Gets the value that indicates whether target file can be overwritten.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Sink_Overwrite")]
        bool Overwrite { get; }

        /// <summary>
        /// Gets the value that indicates whether target file should be compressed.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Sink_Compress")]
        bool Compress { get; }
    }
}
