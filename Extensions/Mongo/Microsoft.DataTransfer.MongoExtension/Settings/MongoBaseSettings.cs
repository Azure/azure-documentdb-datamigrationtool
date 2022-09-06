using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.MongoExtension.Settings;
public class MongoBaseSettings : IDataExtensionSettings
{
    [Required]
    public string? ConnectionString { get; set; }

    [Required]
    public string? DatabaseName { get; set; }
}
