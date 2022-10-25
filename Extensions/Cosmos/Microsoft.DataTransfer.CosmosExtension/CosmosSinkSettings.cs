using Microsoft.Azure.Cosmos;
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
        public int BatchSize { get; set; } = 100;
        public ConnectionMode ConnectionMode { get; set; } = ConnectionMode.Gateway;
        public int MaxRetryCount { get; set; } = 5;
        public int InitialRetryDurationMs { get; set; } = 200;
        public int? CreatedContainerMaxThroughput { get; set; }
        public bool UseAutoscaleForCreatedContainer { get; set; } = true;
        public bool InsertStreams { get; set; } = true;
    }
}