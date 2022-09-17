using System.ComponentModel.DataAnnotations;
using Microsoft.DataTransfer.Interfaces;

namespace Microsoft.DataTransfer.SqlServerExtension
{
    public class SqlServerSourceSettings : IDataExtensionSettings
    {
        [Required]
        public string? ConnectionString { get; set; }

        [Required]
        public string? QueryText { get; set; }

    }
}