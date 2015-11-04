using Microsoft.DataTransfer.DocumentDb.Sink;
using Microsoft.DataTransfer.DocumentDb.Wpf.Shared;
using System.Collections.ObjectModel;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Sink
{
    interface ISharedDocumentDbSinkAdapterConfiguration : ISharedDocumentDbAdapterConfiguration
    {
        ObservableCollection<string> Collections { get; }
        string PartitionKey { get; set; }
        CollectionPricingTier? CollectionTier { get; set; }

        bool UseIndexingPolicyFile { get; set; }
        string IndexingPolicy { get; set; }
        string IndexingPolicyFile { get; set; }

        string IdField { get; set; }
        bool DisableIdGeneration { get; set; }
        DateTimeHandling? Dates { get; set; }
    }
}
