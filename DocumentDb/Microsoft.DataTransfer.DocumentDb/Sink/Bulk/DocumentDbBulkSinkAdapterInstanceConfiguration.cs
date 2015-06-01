
namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    sealed class DocumentDbBulkSinkAdapterInstanceConfiguration : IDocumentDbBulkSinkAdapterInstanceConfiguration
    {
        public string Collection { get; set; }
        public CollectionPricingTier CollectionTier { get; set; }

        public bool DisableIdGeneration { get; set; }
        public string StoredProcName { get; set; }
        public string StoredProcBody { get; set; }
        public int BatchSize { get; set; }
        public int MaxScriptSize { get; set; }
    }
}
