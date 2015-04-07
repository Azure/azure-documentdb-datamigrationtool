using System.Collections.Generic;

namespace Microsoft.DataTransfer.ConsoleHost.UnitTests.DynamicConfiguration
{
    public interface ICollectionConfiguration
    {
        IEnumerable<string> CollectionProperty { get; }
    }
}
