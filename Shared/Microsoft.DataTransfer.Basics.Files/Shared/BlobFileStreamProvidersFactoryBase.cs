using Microsoft.DataTransfer.Basics.Net;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Text.RegularExpressions;

namespace Microsoft.DataTransfer.Basics.Files.Shared
{
    abstract class BlobFileStreamProvidersFactoryBase
    {
        private readonly static Regex BlobAddressThumbprintRegex = new Regex("^blobs?://", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        protected static bool IsBlobAddress(string url)
        {
            return BlobAddressThumbprintRegex.IsMatch(url);
        }

        protected static BlobReference GetBlobReference(string url)
        {
            BlobUri blobUri;
            if (!BlobUri.TryParse(url, out blobUri))
                throw Errors.InvalidBlobUrl();

            return new BlobReference(
                new CloudBlobContainer(
                    blobUri.ContainerUri,
                    new StorageCredentials(blobUri.AccountName, blobUri.AccountKey)),
                blobUri.BlobName);
        }
    }
}
