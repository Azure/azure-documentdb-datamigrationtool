using Azure.Storage.Blobs;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Basics.Files.Source.BlobFile
{
    sealed class BlobFileSourceStreamProvider : ISourceStreamProvider
    {
        private readonly BlobClient blob;

        public string Id
        {
            get { return blob.Uri.ToString(); }
        }

        public BlobFileSourceStreamProvider(BlobClient blob)
        {
            Guard.NotNull("blob", blob);
            this.blob = blob;
        }

        public async Task<Stream> CreateStream(CancellationToken cancellation)
        {
            return await blob.OpenReadAsync(null, cancellation);
        }
    }
}