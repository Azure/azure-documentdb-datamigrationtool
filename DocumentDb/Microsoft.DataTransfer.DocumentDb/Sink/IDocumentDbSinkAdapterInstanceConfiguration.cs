using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Sink
{
    interface IDocumentDbSinkAdapterInstanceConfiguration
    {
        IEnumerable<string> Collections { get; }
        string PartitionKey { get; }
        CollectionPricingTier CollectionTier { get; }
        bool DisableIdGeneration { get; }
    }
}
