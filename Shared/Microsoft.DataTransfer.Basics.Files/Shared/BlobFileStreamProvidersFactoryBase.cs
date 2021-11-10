using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.DataTransfer.Basics.Net;
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
                new BlobContainerClient(
                    blobUri.ContainerUri,
                    new StorageSharedKeyCredential(blobUri.AccountName, blobUri.AccountKey)),
                blobUri.BlobName);
        }
    }
}
