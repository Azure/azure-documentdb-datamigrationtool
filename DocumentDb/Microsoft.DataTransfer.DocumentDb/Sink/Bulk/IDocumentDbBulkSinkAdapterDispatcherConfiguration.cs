using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    interface IDocumentDbBulkSinkAdapterDispatcherConfiguration : IDocumentDbSinkAdapterInstanceConfiguration
    {
        string Database { get; }
        IEnumerable<string> Collections { get; }
        string StoredProcBody { get; }
        int BatchSize { get; }
        int MaxScriptSize { get; }
    }
}
