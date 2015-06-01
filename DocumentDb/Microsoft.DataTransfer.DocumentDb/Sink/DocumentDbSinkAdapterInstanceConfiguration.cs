using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Sink
{
    class DocumentDbSinkAdapterInstanceConfiguration : IDocumentDbSinkAdapterInstanceConfiguration
    {
        public IEnumerable<string> Collections { get; set; }
        public string PartitionKey { get; set; }
        public CollectionPricingTier CollectionTier { get; set; }
        public bool DisableIdGeneration { get; set; }
    }
}
