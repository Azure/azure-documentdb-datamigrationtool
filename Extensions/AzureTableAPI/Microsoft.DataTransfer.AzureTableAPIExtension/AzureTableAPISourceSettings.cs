using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.AzureTableAPIExtension
{
    public class AzureTableAPISourceSettings : IDataExtensionSettings
    {
        [Required]
        public string? FilePath { get; set; }
    }
}