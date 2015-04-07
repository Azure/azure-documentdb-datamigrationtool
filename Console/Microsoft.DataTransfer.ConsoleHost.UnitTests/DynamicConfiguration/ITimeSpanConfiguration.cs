using System;

namespace Microsoft.DataTransfer.ConsoleHost.UnitTests.DynamicConfiguration
{
    public interface ITimeSpanConfiguration
    {
        TimeSpan TimeSpanProperty { get; }
    }
}
