using Microsoft.Azure.Cosmos;
using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.CosmosExtension
{
    public class CosmosSourceSettings : IDataExtensionSettings
    {
        [Required]
        public string? ConnectionString { get; set; }
        [Required]
        public string? Database { get; set; }
        [Required]
        public string? Container { get; set; }

        public string? PartitionKey { get; set; }

        public string? Query { get; set; }

        public bool IncludeMetadataFields { get; set; }
        public ConnectionMode ConnectionMode { get; set; } = ConnectionMode.Gateway;
    }
}