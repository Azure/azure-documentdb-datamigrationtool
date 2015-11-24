using System.Collections.Generic;

namespace Microsoft.DataTransfer.ConsoleHost.Configuration
{
    interface IRawInfrastructureConfiguration
    {
        IReadOnlyDictionary<string, string> InfrastructureConfiguration { get; }
    }
}
