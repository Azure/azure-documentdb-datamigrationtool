
namespace Microsoft.DataTransfer.DocumentDb.Sink.Parallel
{
    sealed class DocumentDbParallelSinkAdapterInstanceConfiguration : DocumentDbSinkAdapterInstanceConfiguration, IDocumentDbParallelSinkAdapterInstanceConfiguration
    {
        public int NumberOfParallelRequests { get; set; }
    }
}
