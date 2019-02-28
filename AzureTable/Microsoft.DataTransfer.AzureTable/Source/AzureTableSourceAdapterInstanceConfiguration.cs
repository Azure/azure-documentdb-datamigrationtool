using Microsoft.DataTransfer.AzureTable.Shared;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.AzureTable.Source
{
    sealed class AzureTableSourceAdapterInstanceConfiguration : IAzureTableSourceAdapterInstanceConfiguration
    {
        public string ConnectionString { get; set; }
        public AzureStorageLocationMode? LocationMode { get; set; }
        public string Table { get; set; }
        public AzureTableInternalFields InternalFields { get; set; }
        public string Filter { get; set; }
        public IEnumerable<string> Projection { get; set; }
    }
}
