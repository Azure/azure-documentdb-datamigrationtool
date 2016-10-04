using Microsoft.Azure.Documents;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    sealed class DocumentDbBulkSinkAdapterInstanceConfiguration : IDocumentDbBulkSinkAdapterInstanceConfiguration
    {
        public string Collection { get; set; }
        public int CollectionThroughput { get; set; }
        public IndexingPolicy IndexingPolicy { get; set; }
        public bool DisableIdGeneration { get; set; }
        public bool UpdateExisting { get; set; }
        public string StoredProcName { get; set; }
        public string StoredProcBody { get; set; }
        public int BatchSize { get; set; }
        public int MaxScriptSize { get; set; }
    }
}
