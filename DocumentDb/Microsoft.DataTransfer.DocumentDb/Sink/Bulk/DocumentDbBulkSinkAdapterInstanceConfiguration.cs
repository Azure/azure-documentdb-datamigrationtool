
namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    sealed class DocumentDbBulkSinkAdapterInstanceConfiguration : DocumentDbSinkAdapterInstanceConfiguration, IDocumentDbBulkSinkAdapterInstanceConfiguration
    {
        public string StoredProcBody { get; set; }
        public int BatchSize { get; set; }
        public int MaxScriptSize { get; set; }
    }
}
