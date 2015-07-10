using Microsoft.Azure.Documents;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    interface IDocumentDbBulkSinkAdapterInstanceConfiguration
    {
        string Collection { get; }
        CollectionPricingTier CollectionTier { get; }
        IndexingPolicy IndexingPolicy { get; }
        bool DisableIdGeneration { get; }
        string StoredProcName { get; }
        string StoredProcBody { get; }
        int BatchSize { get; }
        int MaxScriptSize { get; }
    }
}
