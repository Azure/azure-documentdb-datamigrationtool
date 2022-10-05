using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.AzureTableAPIExtension.Settings
{
    public abstract class AzureTableAPISettingsBase : IDataExtensionSettings
    {
        /// <summary>
        /// The Connection String.
        /// </summary>
        [Required]
        public string? ConnectionString { get; set; }

        /// <summary>
        /// The Table name.
        /// </summary>
        [Required]
        public string? Table { get; set; }

        /// <summary>
        /// The field name to translate the RowKey from Table Entities to. (Optional)
        /// </summary>
        public string? RowKeyFieldName { get; set; }

        /// <summary>
        /// The field name to translate the PartitionKey from Table Entities to. (Optional)
        /// </summary>
        public string? PartitionKeyFieldName { get; set; }
    }
}