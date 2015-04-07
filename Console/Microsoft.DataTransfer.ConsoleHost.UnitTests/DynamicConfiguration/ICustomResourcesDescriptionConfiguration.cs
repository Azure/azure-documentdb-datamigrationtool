using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.ConsoleHost.UnitTests.DynamicConfiguration
{
    public interface ICustomResourcesDescriptionConfiguration
    {
        [Display(ResourceType = typeof(DescriptionResources), Description = "PropADescription")]
        string PropA { get; }

        [Display(ResourceType = typeof(DescriptionResources), Description = "PropBDescription")]
        int PropB { get; }
    }
}
