using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.MongoExtension.Settings;
public class MongoSourceSettings : MongoBaseSettings
{
    public string? Collection { get; set; }
}
