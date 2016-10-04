using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    sealed class DocumentDbBulkSinkAdapterDispatcherConfiguration : DocumentDbSinkAdapterInstanceConfiguration, IDocumentDbBulkSinkAdapterDispatcherConfiguration
    {
        public IEnumerable<string> Collections { get; set; }
        public string StoredProcBody { get; set; }
        public int BatchSize { get; set; }
        public int MaxScriptSize { get; set; }
    }
}
