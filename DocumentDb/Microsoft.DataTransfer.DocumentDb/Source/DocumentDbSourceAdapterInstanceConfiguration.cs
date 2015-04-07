using Microsoft.DataTransfer.DocumentDb.Shared;

namespace Microsoft.DataTransfer.DocumentDb.Source
{
    sealed class DocumentDbSourceAdapterInstanceConfiguration : DocumentDbAdapterInstanceConfiguration, IDocumentDbSourceAdapterInstanceConfiguration
    {
        public string Query { get; set; }
    }
}
