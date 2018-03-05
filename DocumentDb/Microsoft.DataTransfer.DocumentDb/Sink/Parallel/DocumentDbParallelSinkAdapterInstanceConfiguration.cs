
namespace Microsoft.DataTransfer.DocumentDb.Sink.Parallel
{
    sealed class DocumentDbParallelSinkAdapterInstanceConfiguration : DocumentDbSinkAdapterInstanceConfiguration, IDocumentDbParallelSinkAdapterInstanceConfiguration
    {
        public string Database { get; set; }
        public string Collection { get; set; }
        public int NumberOfParallelRequests { get; set; }
    }
}
