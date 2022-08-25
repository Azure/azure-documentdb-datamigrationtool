using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.JsonExtension
{
    public class JsonSourceSettings : IDataExtensionSettings
    {
        [Required]
        public string? FilePath { get; set; }
    }
}