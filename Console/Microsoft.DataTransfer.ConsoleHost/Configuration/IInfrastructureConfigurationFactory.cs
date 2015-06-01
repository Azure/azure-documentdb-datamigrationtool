using System.Collections.Generic;

namespace Microsoft.DataTransfer.ConsoleHost.Configuration
{
    interface IInfrastructureConfigurationFactory
    {
        IInfrastructureConfiguration Create(IReadOnlyDictionary<string, string> arguments);
        IReadOnlyDictionary<string, string> DescribeOptions();
    }
}
