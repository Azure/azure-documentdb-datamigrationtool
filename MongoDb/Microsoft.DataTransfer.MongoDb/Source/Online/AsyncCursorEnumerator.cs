using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility.Basics.Collections;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.MongoDb.Source.Online
{
    sealed class AsyncCursorEnumerator<TDocument> : IAsyncEnumerator<TDocument>
    {
        private IAsyncCursor<TDocument> asyncCursor;
        private IEnumerator<TDocument> batchCursor;
        private TDocument currentItem;

        private Task<bool> batchDownloadTask;

        public TDocument Current
        {
            get { return currentItem; }
        }

        public AsyncCursorEnumerator(IAsyncCursor<TDocument> asyncCursor)
        {
            Guard.NotNull("asyncCursor", asyncCursor);

            this.asyncCursor = asyncCursor;
        }

        public async Task<bool> MoveNextAsync(CancellationToken cancellation)
        {
            if (batchDownloadTask == null)
            {
                batchDownloadTask = MoveToNextBatch(cancellation);
            }

            if (!await batchDownloadTask)
                return false;

            currentItem = batchCursor.Current;

            if (!batchCursor.MoveNext())
            {
                batchDownloadTask = MoveToNextBatch(cancellation);
            }

            return true;
        }

        private async Task<bool> MoveToNextBatch(CancellationToken cancellation)
        {
            while (await asyncCursor.MoveNextAsync(cancellation))
            {
                TrashCan.Throw(ref batchCursor);

                if (asyncCursor.Current == null)
                {
                    continue;
                }

                batchCursor = asyncCursor.Current.GetEnumerator();

                if (batchCursor.MoveNext())
                {
                    return true;
                }
            }

            return false;
        }

        public void Dispose()
        {
            TrashCan.Throw(ref batchCursor);
            TrashCan.Throw(ref asyncCursor);
        }
    }
}
