using System.ComponentModel.DataAnnotations;
using Microsoft.DataTransfer.Interfaces;

namespace Microsoft.DataTransfer.SqlServerExtension
{
    public class SqlServerSinkSettings : IDataExtensionSettings
    {
        [Required]
        public string? ConnectionString { get; set; }
        [Required]
        public string? TableName { get; set; }

        [Range(1, int.MaxValue)]
        public int BatchSize { get; set; } = 1000;

        [MinLength(1)]
        public List<ColumnMapping> ColumnMappings { get; set; } = new List<ColumnMapping>();

    }
}