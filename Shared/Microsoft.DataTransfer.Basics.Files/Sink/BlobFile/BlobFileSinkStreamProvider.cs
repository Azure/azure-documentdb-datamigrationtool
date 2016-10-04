using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Basics.Files.Sink.BlobFile
{
    sealed class BlobFileSinkStreamProvider : ISinkStreamProvider
    {
        private readonly CloudBlockBlob blob;
        private readonly bool overwrite;

        public BlobFileSinkStreamProvider(CloudBlockBlob blob, bool overwrite)
        {
            Guard.NotNull("blob", blob);

            this.blob = blob;
            this.overwrite = overwrite;
        }

        public async Task<Stream> CreateStream(CancellationToken cancellation)
        {
            if (await blob.ExistsAsync() && !overwrite)
                throw Errors.BlobAlreadyExists(blob.Uri.ToString());

            await blob.Container.CreateIfNotExistsAsync(cancellation);

            return new BlobStream(
                await blob.OpenWriteAsync(
                    overwrite ? null : AccessCondition.GenerateIfNoneMatchCondition("*"), null, null, cancellation),
                blob.Uri.ToString());
        }
    }
}
