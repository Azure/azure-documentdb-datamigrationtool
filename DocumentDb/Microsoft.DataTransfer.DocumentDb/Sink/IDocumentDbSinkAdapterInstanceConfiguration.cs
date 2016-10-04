using Microsoft.Azure.Documents;

namespace Microsoft.DataTransfer.DocumentDb.Sink
{
    interface IDocumentDbSinkAdapterInstanceConfiguration
    {
        int CollectionThroughput { get; }
        string PartitionKey { get; }
        IndexingPolicy IndexingPolicy { get; }
        bool DisableIdGeneration { get; }
        bool UpdateExisting { get; }
    }
}
