using Microsoft.Azure.Documents.Linq;
using Microsoft.DataTransfer.DocumentDb.Client.Serialization;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Client.Enumeration
{
    sealed class DocumentSurrogateQueryAsyncEnumerator : AsyncEnumeratorBase<DocumentSurrogate, IReadOnlyDictionary<string, object>>
    {
        public DocumentSurrogateQueryAsyncEnumerator(IDocumentQuery<DocumentSurrogate> documentQuery)
            : base(documentQuery) {}

        protected override IReadOnlyDictionary<string, object> ToOutputItem(DocumentSurrogate input)
        {
            return input.Properties;
        }
    }
}
