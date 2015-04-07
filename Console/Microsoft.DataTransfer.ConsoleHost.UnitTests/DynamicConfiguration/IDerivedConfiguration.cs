
namespace Microsoft.DataTransfer.ConsoleHost.UnitTests.DynamicConfiguration
{
    public interface IDerivedConfiguration : ISimpleConfiguration
    {
        string NewProperty1 { get; }
    }
}
