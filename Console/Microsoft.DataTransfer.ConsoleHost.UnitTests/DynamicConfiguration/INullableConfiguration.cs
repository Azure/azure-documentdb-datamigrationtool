
namespace Microsoft.DataTransfer.ConsoleHost.UnitTests.DynamicConfiguration
{
    public interface INullableConfiguration
    {
        int? IntProp { get; }
        TestEnum? EnumProp { get; }
    }
}
