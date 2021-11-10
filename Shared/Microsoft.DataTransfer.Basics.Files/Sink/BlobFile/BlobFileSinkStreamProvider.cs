using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Basics.Files.Sink.BlobFile
{
    sealed class BlobFileSinkStreamProvider : ISinkStreamProvider
    {
        private readonly BlobClient blob;
        private readonly bool overwrite;

        public BlobFileSinkStreamProvider(BlobClient blob, bool overwrite)
        {           
            Guard.NotNull("blob", blob);
                       
            this.blob = blob;
            this.overwrite = overwrite;
        }

        public async Task<Stream> CreateStream(CancellationToken cancellation)
        {
            if (await blob.ExistsAsync() && !overwrite)
                throw Errors.BlobAlreadyExists(blob.Uri.ToString());

            var containerClient = blob.GetParentBlobContainerClient();
            await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellation);

            var blockBlobClient = containerClient.GetBlockBlobClient(blob.Name);
            return new BlobStream(
                await blockBlobClient.OpenWriteAsync(overwrite,null, cancellation),
                blob.Uri.ToString());
        }
    }
}
