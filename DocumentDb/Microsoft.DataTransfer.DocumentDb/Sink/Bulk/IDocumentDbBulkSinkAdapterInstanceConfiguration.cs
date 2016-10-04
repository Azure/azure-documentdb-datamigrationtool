using Microsoft.Azure.Documents;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    interface IDocumentDbBulkSinkAdapterInstanceConfiguration
    {
        string Collection { get; }
        int CollectionThroughput { get; }
        IndexingPolicy IndexingPolicy { get; }
        bool DisableIdGeneration { get; }
        bool UpdateExisting { get; }
        string StoredProcName { get; }
        string StoredProcBody { get; }
        int BatchSize { get; }
        int MaxScriptSize { get; }
    }
}
