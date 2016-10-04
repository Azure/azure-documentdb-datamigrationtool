
namespace Microsoft.DataTransfer.DocumentDb.Sink.Parallel
{
    interface IDocumentDbParallelSinkAdapterInstanceConfiguration : IDocumentDbSinkAdapterInstanceConfiguration
    {
        string Collection { get; }
        int NumberOfParallelRequests { get; }
    }
}
