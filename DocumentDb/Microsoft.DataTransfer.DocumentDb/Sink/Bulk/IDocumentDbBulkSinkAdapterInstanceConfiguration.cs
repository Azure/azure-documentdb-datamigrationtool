
namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    interface IDocumentDbBulkSinkAdapterInstanceConfiguration : IDocumentDbSinkAdapterInstanceConfiguration
    {
        string StoredProcBody { get; }
        int BatchSize { get; }
        int MaxScriptSize { get; }
    }
}
