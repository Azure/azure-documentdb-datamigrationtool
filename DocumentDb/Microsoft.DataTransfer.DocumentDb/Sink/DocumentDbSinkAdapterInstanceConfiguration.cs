using Microsoft.Azure.Documents;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Sink
{
    class DocumentDbSinkAdapterInstanceConfiguration : IDocumentDbSinkAdapterInstanceConfiguration
    {
        public IEnumerable<string> Collections { get; set; }
        public string PartitionKey { get; set; }
        public CollectionPricingTier CollectionTier { get; set; }
        public IndexingPolicy IndexingPolicy { get; set; }
        public bool DisableIdGeneration { get; set; }
        public bool UpdateExisting { get; set; }
    }
}
