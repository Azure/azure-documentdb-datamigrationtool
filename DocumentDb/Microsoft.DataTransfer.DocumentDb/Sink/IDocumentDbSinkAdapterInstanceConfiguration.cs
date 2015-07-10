using Microsoft.Azure.Documents;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Sink
{
    interface IDocumentDbSinkAdapterInstanceConfiguration
    {
        IEnumerable<string> Collections { get; }
        string PartitionKey { get; }
        CollectionPricingTier CollectionTier { get; }
        IndexingPolicy IndexingPolicy { get; }
        bool DisableIdGeneration { get; }
    }
}
