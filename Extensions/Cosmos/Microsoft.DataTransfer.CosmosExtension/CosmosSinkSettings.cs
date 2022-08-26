using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.CosmosExtension
{
    public class CosmosSinkSettings : IDataExtensionSettings
    {
        [Required]
        public string? ConnectionString { get; set; }
        [Required]
        public string? Database { get; set; }
        [Required]
        public string? Container { get; set; }
        [Required]
        public string? PartitionKeyPath { get; set; }
        public bool RecreateContainer { get; set; }
    }
}