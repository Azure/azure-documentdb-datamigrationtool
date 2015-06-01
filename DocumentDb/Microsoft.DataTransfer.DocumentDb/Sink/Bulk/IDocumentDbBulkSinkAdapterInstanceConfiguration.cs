
namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    interface IDocumentDbBulkSinkAdapterInstanceConfiguration
    {
        string Collection { get; }
        CollectionPricingTier CollectionTier { get; }
        bool DisableIdGeneration { get; }
        string StoredProcName { get; }
        string StoredProcBody { get; }
        int BatchSize { get; }
        int MaxScriptSize { get; }
    }
}
