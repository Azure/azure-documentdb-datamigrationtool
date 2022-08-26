using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.AzureTableAPIExtension
{
    public class AzureTableAPISourceSettings : IDataExtensionSettings
    {
        [Required]
        public string? ConnectionString { get; set; }

        [Required]
        public string? Table { get; set; }

        public string? QueryFilter { get; set; }
    }
}