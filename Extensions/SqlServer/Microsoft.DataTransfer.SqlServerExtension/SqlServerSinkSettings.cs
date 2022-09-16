using System.ComponentModel.DataAnnotations;
using Microsoft.DataTransfer.Interfaces;

namespace Microsoft.DataTransfer.SqlServerExtension
{
    public class SqlServerSinkSettings : IDataExtensionSettings
    {
        [Required]
        public string? ConnectionString { get; set; }
    }
}