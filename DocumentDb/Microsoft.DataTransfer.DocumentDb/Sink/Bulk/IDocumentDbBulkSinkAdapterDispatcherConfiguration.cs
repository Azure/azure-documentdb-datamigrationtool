
namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    interface IDocumentDbBulkSinkAdapterDispatcherConfiguration : IDocumentDbSinkAdapterInstanceConfiguration
    {
        string StoredProcBody { get; }
        int BatchSize { get; }
        int MaxScriptSize { get; }
    }
}
