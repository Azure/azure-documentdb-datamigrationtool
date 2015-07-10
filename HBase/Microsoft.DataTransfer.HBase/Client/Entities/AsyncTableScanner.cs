using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility.Basics.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.HBase.Client.Entities
{
    sealed class AsyncTableScanner : IAsyncEnumerator<HBaseRow>, IScannerReference
    {
        private IStargateScanClient client;

        private Task<IReadOnlyList<HBaseRow>> chunkDownloadTask;
        private int currentRowIndex;
        private HBaseRow next;

        public string TableName { get; private set; }
        public string ScannerId { get; private set; }

        public HBaseRow Current { get; private set; }

        public AsyncTableScanner(IStargateScanClient client, string tableName, string scannerId)
        {
            Guard.NotNull("client", client);
            Guard.NotNull("tableName", tableName);
            Guard.NotNull("scannerId", scannerId);

            this.client = client;

            TableName = tableName;
            ScannerId = scannerId;
        }

        public async Task<bool> MoveNextAsync(CancellationToken cancellation)
        {
            if (chunkDownloadTask == null)
            {
                MoveToNextChunk(cancellation);
            }

            var currentChunk = await chunkDownloadTask;

            do
            {
                // Make sure current chunk has data
                while (currentChunk != null && currentRowIndex >= currentChunk.Count)
                {
                    MoveToNextChunk(cancellation);
                    currentChunk = await chunkDownloadTask;
                }

                if (currentChunk == null)
                    break;

                var currentRow = currentChunk[currentRowIndex++];

                if (next == null)
                {
                    next = currentRow;
                }
                else if (next.Key == currentRow.Key)
                {
                    next.Cells.AddRange(currentRow.Cells);
                }
                else
                {
                    // Different row id - rollback and preserve it in next field later.
                    --currentRowIndex;
                    break;
                }
            } while (currentRowIndex >= currentChunk.Count);

            Current = next;
            next = null;

            if (currentChunk != null)
            {
                if (currentRowIndex < currentChunk.Count)
                    next = currentChunk[currentRowIndex];

                if (currentRowIndex >= currentChunk.Count - 1)
                    // Start downloading next chunk while current row is being processed    
                    MoveToNextChunk(cancellation);
            }

            return Current != null;
        }

        private void MoveToNextChunk(CancellationToken cancellation)
        {
            chunkDownloadTask = client.ScanNextChunkAsync(this, cancellation);
            currentRowIndex = 0;
        }

        public void Dispose()
        {
            client.CloseScannerAsync(this, CancellationToken.None).Wait();
        }
    }
}
