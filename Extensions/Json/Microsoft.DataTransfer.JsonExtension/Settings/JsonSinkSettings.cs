using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.JsonExtension.Settings
{
    public class JsonSinkSettings : IDataExtensionSettings
    {
        [Required]
        public string? FilePath { get; set; }

        public bool IncludeNullFields { get; set; }
        public bool Indented { get; set; }
    }
}