using Microsoft.Azure.Documents;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Sink
{
    interface IDocumentDbSinkAdapterInstanceConfiguration
    {
        string PartitionKey { get; }
        IndexingPolicy IndexingPolicy { get; }
        bool DisableIdGeneration { get; }
        bool UpdateExisting { get; }
    }
}
