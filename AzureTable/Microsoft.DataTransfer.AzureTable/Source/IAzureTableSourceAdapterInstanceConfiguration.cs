using Microsoft.DataTransfer.AzureTable.Shared;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.AzureTable.Source
{
    interface IAzureTableSourceAdapterInstanceConfiguration : IAzureTableAdapterConfiguration
    {
        string Table { get; }
        AzureTableInternalFields InternalFields { get; }
        string Filter { get; }
        IEnumerable<string> Projection { get; }
    }
}
