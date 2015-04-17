
using Microsoft.DataTransfer.RavenDb.Shared;

namespace Microsoft.DataTransfer.RavenDb.Source
{
    interface IRavenDbSourceAdapterInstanceConfiguration : IRavenDbDataAdapterConfiguration
    {
        string Collection { get; }
    }
}
