using System.Collections.Generic;

namespace Microsoft.DataTransfer.ConsoleHost.Configuration
{
    interface IOneTimeDataTransferConfiguration
    {
        string SourceName { get; }
        IReadOnlyDictionary<string, string> SourceConfiguration { get; }

        string TargetName { get; }
        IReadOnlyDictionary<string, string> TargetConfiguration { get; }
    }
}
