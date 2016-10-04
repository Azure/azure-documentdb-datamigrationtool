
namespace Microsoft.DataTransfer.DocumentDb.Sink.Parallel
{
    sealed class DocumentDbParallelSinkAdapterInstanceConfiguration : DocumentDbSinkAdapterInstanceConfiguration, IDocumentDbParallelSinkAdapterInstanceConfiguration
    {
        public string Collection { get; set; }
        public int NumberOfParallelRequests { get; set; }
    }
}
