using Microsoft.DataTransfer.DocumentDb.Shared;

namespace Microsoft.DataTransfer.DocumentDb.Sink
{
    interface IDocumentDbSinkAdapterInstanceConfiguration : IDocumentDbAdapterInstanceConfiguration
    {
        CollectionPricingTier CollectionTier { get; }
        bool DisableIdGeneration { get; }
    }
}
