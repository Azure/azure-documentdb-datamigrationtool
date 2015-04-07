
namespace Microsoft.DataTransfer.DocumentDb.Sink.Parallel
{
    interface IDocumentDbParallelSinkAdapterInstanceConfiguration : IDocumentDbSinkAdapterInstanceConfiguration
    {
        int NumberOfParallelRequests { get; }
    }
}
