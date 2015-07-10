using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Basics.Files.Source.BlobFile
{
    sealed class BlobFileSourceStreamProvider : ISourceStreamProvider
    {
        private readonly ICloudBlob blob;

        public string Id
        {
            get { return blob.Uri.ToString(); }
        }

        public BlobFileSourceStreamProvider(ICloudBlob blob)
        {
            Guard.NotNull("blob", blob);
            this.blob = blob;
        }

        public async Task<StreamReader> CreateReader(CancellationToken cancellation)
        {
            return new StreamReader(await blob.OpenReadAsync(cancellation));
        }
    }
}
