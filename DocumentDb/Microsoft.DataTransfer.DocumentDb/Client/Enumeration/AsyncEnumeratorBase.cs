using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.DataTransfer.Basics;
using System.Collections.Generic;
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

        public async Task<bool> MoveNextAsync()
        {
            if (completed)
                return false;

            if (chunkDownloadTask == null)
            {
                RequestNextChunk();
            }

            var currentCursor = await GetCurrentChunkCursor();
            if (currentCursor == null)
                return false;

            Current = ToOutputItem(currentCursor.Current);

            if (!currentCursor.MoveNext() && !(completed = !documentQuery.HasMoreResults))
            {
                RequestNextChunk();
            }

            return true;
        }

        protected abstract TOut ToOutputItem(TIn input);

        private async Task<IEnumerator<TIn>> GetCurrentChunkCursor()
        {
            if (chunkCursor == null)
            {
                chunkCursor = (await chunkDownloadTask).GetEnumerator();

                // New chunk: adjust to first record to make sure that there is data to read, if not - request next one
                var hasData = false;
                while (!(hasData = chunkCursor.MoveNext()) && documentQuery.HasMoreResults)
                {
                    RequestNextChunk();
                    chunkCursor = (await chunkDownloadTask).GetEnumerator();
                }

                completed = !hasData;
                if (completed)
                    TrashCan.Throw(ref chunkCursor);
            }

            return chunkCursor;
        }

        private void RequestNextChunk()
        {
            TrashCan.Throw(ref chunkCursor);
            chunkDownloadTask = documentQuery.ExecuteNextAsync<TIn>();
        }

        public virtual void Dispose()
        {
            TrashCan.Throw(ref chunkCursor);
        }
    }
}
