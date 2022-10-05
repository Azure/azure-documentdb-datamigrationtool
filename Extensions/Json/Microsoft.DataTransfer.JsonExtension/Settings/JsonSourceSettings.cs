using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.JsonExtension.Settings
{
    public class JsonSourceSettings : IDataExtensionSettings
    {
        [Required]
        public string? FilePath { get; set; }
    }
}