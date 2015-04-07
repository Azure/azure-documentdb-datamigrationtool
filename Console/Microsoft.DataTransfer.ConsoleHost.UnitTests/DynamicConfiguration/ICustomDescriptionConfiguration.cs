using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.ConsoleHost.UnitTests.DynamicConfiguration
{
    public interface ICustomDescriptionConfiguration
    {
        [Display(Description = "Hello")]
        string PropA { get; }

        [Display(Description = "World!")]
        int PropB { get; }
    }
}
