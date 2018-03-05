
namespace Microsoft.DataTransfer.DocumentDb.Sink.Parallel
{
    interface IDocumentDbParallelSinkAdapterInstanceConfiguration : IDocumentDbSinkAdapterInstanceConfiguration
    {
        string Database { get; }
        string Collection { get; }
        int NumberOfParallelRequests { get; }
    }
}
