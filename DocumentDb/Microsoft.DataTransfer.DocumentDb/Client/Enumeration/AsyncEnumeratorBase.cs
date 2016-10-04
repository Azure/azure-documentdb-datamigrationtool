using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility.Basics.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Client.Enumeration
{
    abstract class AsyncEnumeratorBase<TIn, TOut> : IAsyncEnumerator<TOut>
    {
        private IDocumentQuery<TIn> documentQuery;

        private bool completed;

        private Task<FeedResponse<TIn>> chunkDownloadTask;
        private IEnumerator<TIn> chunkCursor;

        public TOut Current { get; private set; }

        public AsyncEnumeratorBase(IDocumentQuery<TIn> documentQuery)
        {
            Guard.NotNull("documentQuery", documentQuery);

            this.documentQuery = documentQuery;
            completed = false;
        }

        public async Task<bool> MoveNextAsync(CancellationToken cancellation)
        {
            if (completed)
                return false;

            if (chunkDownloadTask == null)
            {
                RequestNextChunk(cancellation);
            }

            var currentCursor = await GetCurrentChunkCursor(cancellation);
            if (currentCursor == null)
                return false;

            Current = ToOutputItem(currentCursor.Current);

            if (!currentCursor.MoveNext() && !(completed = !documentQuery.HasMoreResults))
            {
                RequestNextChunk(cancellation);
            }

            return true;
        }

        protected abstract TOut ToOutputItem(TIn input);

        private async Task<IEnumerator<TIn>> GetCurrentChunkCursor(CancellationToken cancellation)
        {
            if (chunkCursor == null)
            {
                chunkCursor = (await chunkDownloadTask).GetEnumerator();

                // New chunk: adjust to first record to make sure that there is data to read, if not - request next one
                var hasData = false;
                while (!(hasData = chunkCursor.MoveNext()) && documentQuery.HasMoreResults)
                {
                    RequestNextChunk(cancellation);
                    chunkCursor = (await chunkDownloadTask).GetEnumerator();
                }

                completed = !hasData;
                if (completed)
                    TrashCan.Throw(ref chunkCursor);
            }

            return chunkCursor;
        }

        private void RequestNextChunk(CancellationToken cancellation)
        {
            TrashCan.Throw(ref chunkCursor);
            chunkDownloadTask = documentQuery.ExecuteNextAsync<TIn>(cancellation);
        }

        public virtual void Dispose()
        {
            TrashCan.Throw(ref chunkCursor);
        }
    }
}
