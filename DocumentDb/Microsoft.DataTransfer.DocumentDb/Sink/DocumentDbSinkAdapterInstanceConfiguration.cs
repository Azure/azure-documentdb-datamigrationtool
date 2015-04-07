using Microsoft.DataTransfer.DocumentDb.Shared;

namespace Microsoft.DataTransfer.DocumentDb.Sink
{
    class DocumentDbSinkAdapterInstanceConfiguration : DocumentDbAdapterInstanceConfiguration, IDocumentDbSinkAdapterInstanceConfiguration
    {
        public CollectionPricingTier CollectionTier { get; set; }
        public bool DisableIdGeneration { get; set; }
    }
}
