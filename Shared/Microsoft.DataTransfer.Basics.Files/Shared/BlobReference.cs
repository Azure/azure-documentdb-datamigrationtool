using Microsoft.WindowsAzure.Storage.Blob;

namespace Microsoft.DataTransfer.Basics.Files.Shared
{
    sealed class BlobReference
    {
        public CloudBlobContainer Container { get; private set; }
        public string BlobName { get; private set; }

        public BlobReference(CloudBlobContainer container, string blobName)
        {
            Guard.NotNull("container", container);

            Container = container;
            BlobName = blobName;
        }
    }
}
