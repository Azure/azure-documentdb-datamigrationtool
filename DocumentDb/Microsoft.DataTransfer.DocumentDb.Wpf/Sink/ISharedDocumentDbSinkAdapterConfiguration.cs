using Microsoft.DataTransfer.DocumentDb.Sink;
using Microsoft.DataTransfer.DocumentDb.Wpf.Shared;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Sink
{
    interface ISharedDocumentDbSinkAdapterConfiguration : ISharedDocumentDbAdapterConfiguration
    {
        int? CollectionThroughput { get; set; }
        bool UseIndexingPolicyFile { get; set; }
        string IndexingPolicy { get; set; }
        string IndexingPolicyFile { get; set; }

        string IdField { get; set; }
        bool DisableIdGeneration { get; set; }
        bool UpdateExisting { get; set; }
        DateTimeHandling? Dates { get; set; }
    }
}
