using Microsoft.Azure.Documents.Linq;

namespace Microsoft.DataTransfer.DocumentDb.Client.Enumeration
{
    sealed class AsyncEnumerator<T> : AsyncEnumeratorBase<T, T>
    {
        public AsyncEnumerator(IDocumentQuery<T> documentQuery)
            : base(documentQuery) { }

        protected override T ToOutputItem(T input)
        {
            return input;
        }
    }
}
