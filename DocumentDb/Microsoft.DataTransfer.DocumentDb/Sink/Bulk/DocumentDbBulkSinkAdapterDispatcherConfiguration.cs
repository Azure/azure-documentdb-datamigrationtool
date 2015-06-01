
namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    sealed class DocumentDbBulkSinkAdapterDispatcherConfiguration : DocumentDbSinkAdapterInstanceConfiguration, IDocumentDbBulkSinkAdapterDispatcherConfiguration
    {
        public string StoredProcBody { get; set; }
        public int BatchSize { get; set; }
        public int MaxScriptSize { get; set; }
    }
}
