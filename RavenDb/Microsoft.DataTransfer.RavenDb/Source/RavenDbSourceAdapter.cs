using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Raven.Abstractions.Data;
using Raven.Abstractions.Util;
using Raven.Client.Document;
using Raven.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.RavenDb.Source
{
    sealed class RavenDbSourceAdapter : IDataSourceAdapter
    {
        private const string MetadataIdField = "@id";
        private const string DocumentIdField = "__document_id";

        private IRavenDbSourceAdapterInstanceConfiguration configuration;

        private IAsyncEnumerator<StreamResult<RavenJObject>> documentsCursor;

        public RavenDbSourceAdapter(IRavenDbSourceAdapterInstanceConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);
            this.configuration = configuration;
        }

        public async Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation)
        {
            if (documentsCursor == null)
            {
                await InitializeDocumentsCursor(cancellation);
            }

            if (!await documentsCursor.MoveNextAsync())
                return null;

            RavenJToken idToken = null;
            if (documentsCursor.Current.Metadata != null && documentsCursor.Current.Metadata.TryGetValue(MetadataIdField, out idToken))
                readOutput.DataItemId = idToken.Value<string>();

            var jObject = documentsCursor.Current.Document as RavenJObject;
            if (jObject == null)
                throw NonFatalReadException.Convert(Errors.NonJsonDocumentRead());

            if (!configuration.ExcludeIdField && idToken != null)
                jObject.Add(DocumentIdField, idToken);

            return new RavenJObjectDataItem(jObject);
        }

        private async Task InitializeDocumentsCursor(CancellationToken cancellation)
        {
            using (var store = new DocumentStore())
            {
                store.ParseConnectionString(configuration.ConnectionString);
                store.Initialize(false);

                using (var session = store.OpenAsyncSession())
                {
                    if (String.IsNullOrEmpty(configuration.Query))
                    {
                        documentsCursor = await session.Advanced.StreamAsync<RavenJObject>(Etag.Empty, 0, int.MaxValue, null, cancellation);
                    }
                    else
                    {
                        var query = String.IsNullOrEmpty(configuration.Index)
                            ? session.Advanced.AsyncDocumentQuery<RavenJObject>()
                            : session.Advanced.AsyncDocumentQuery<RavenJObject>(configuration.Index);

                        // Streaming query API does not create a dynamic index, so user have to specify one if query is provided: http://issues.hibernatingrhinos.com/issue/RavenDB-1410
                        documentsCursor = await session.Advanced.StreamAsync<RavenJObject>(
                            query.Where(configuration.Query).NoCaching().NoTracking(), cancellation);
                    }
                }
            }
        }

        public void Dispose()
        {
            TrashCan.Throw(ref documentsCursor);
        }
    }
}
