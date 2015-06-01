using Microsoft.DataTransfer.RavenDb.Shared;

namespace Microsoft.DataTransfer.RavenDb.Source
{
    interface IRavenDbSourceAdapterInstanceConfiguration : IRavenDbAdapterConfiguration
    {
        string Index { get; }
        string Query { get; }
        bool ExcludeIdField { get; }
    }
}
