using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.AzureTableAPIExtension.Settings
{
    public abstract class AzureTableAPISettingsBase : IDataExtensionSettings
    {
        /// <summary>
        /// The Connection String.
        /// </summary>
        [Required]
        public string? ConnectionString { get; set; }

        /// <summary>
        /// The Table name.
        /// </summary>
        [Required]
        public string? Table { get; set; }
    }
}