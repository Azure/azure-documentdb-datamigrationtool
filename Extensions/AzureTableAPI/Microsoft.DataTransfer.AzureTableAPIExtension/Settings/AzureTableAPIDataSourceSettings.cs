using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.AzureTableAPIExtension.Settings
{
    public class AzureTableAPIDataSinkSettings : AzureTableAPISettingsBase
    {
        [Required]
        public string? PartitionKeyFieldName { get; set; }

        [Required]
        public string? RowKeyFieldName { get; set; }
    }
}