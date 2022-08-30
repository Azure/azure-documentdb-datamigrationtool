using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.MongoExtension.Settings;
public class MongoSinkSettings : MongoBaseSettings
{
    [Required]
    public string? Collection { get; set; }

    public int? BatchSize { get; set; }
}
