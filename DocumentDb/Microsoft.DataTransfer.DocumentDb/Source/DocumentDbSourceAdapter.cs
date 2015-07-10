using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.DocumentDb.Client;
using Microsoft.DataTransfer.DocumentDb.Shared;
using Microsoft.DataTransfer.DocumentDb.Transformation;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Collections;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Source
{
    sealed class DocumentDbSourceAdapter : DocumentDbAdapterBase<IDocumentDbReadClient, IDocumentDbSourceAdapterInstanceConfiguration>, IDataSourceAdapter
    {
        private const string DocumentIdFieldName = "id";

        private IAsyncEnumerator<IReadOnlyDictionary<string, object>> documentsCursor;

        public DocumentDbSourceAdapter(IDocumentDbReadClient client, IDataItemTransformation transformation, IDocumentDbSourceAdapterInstanceConfiguration configuration)
            : base(client, transformation, configuration) { }

        public async Task InitializeAsync(CancellationToken cancellation)
        {
            documentsCursor = await Client
                .QueryDocumentsAsync(Configuration.Collection, Configuration.Query, cancellation);
        }

        public async Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation)
        {
            if (documentsCursor == null)
                throw Errors.SourceIsNotInitialized();

            if (!(await documentsCursor.MoveNextAsync(cancellation)))
                return null;

            var document = documentsCursor.Current;

            object idValue;
            if (document.TryGetValue(DocumentIdFieldName, out idValue))
                readOutput.DataItemId = idValue.ToString();

            return Transformation.Transform(new DictionaryDataItem(document));
        }

        public override void Dispose()
        {
            TrashCan.Throw(ref documentsCursor);
            base.Dispose();
        }
    }
}
