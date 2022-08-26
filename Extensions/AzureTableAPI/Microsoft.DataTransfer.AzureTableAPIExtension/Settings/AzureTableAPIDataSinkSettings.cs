using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.AzureTableAPIExtension.Settings
{
    public class AzureTableAPISourceSettings : AzureTableAPISettingsBase
    {
        public string? QueryFilter { get; set; }
    }
}