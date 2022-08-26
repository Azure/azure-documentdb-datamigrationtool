using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.MongoExtension;
public class MongoSourceSettings : IDataExtensionSettings
{
    [Required]
    public string? ConnectionString { get; set; }

    [Required]
    public string? DatabaseName { get; set; }

    public string? Collection { get; set; }
}
