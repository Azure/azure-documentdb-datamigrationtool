using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Source
{
    sealed class DocumentDbSourceAdapterInstanceConfiguration : IDocumentDbSourceAdapterInstanceConfiguration
    {
        public string Database { get; set; }
        public string Collection { get; set; }
        public string Query { get; set; }
    }
}
