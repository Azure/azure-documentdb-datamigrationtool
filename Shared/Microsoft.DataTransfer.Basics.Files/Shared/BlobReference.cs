using Azure.Storage.Blobs;

namespace Microsoft.DataTransfer.Basics.Files.Shared
{
    sealed class BlobReference
    {
        public BlobContainerClient Container { get; private set; }
        public string BlobName { get; private set; }

        public BlobReference(BlobContainerClient container, string blobName)
        {
            Guard.NotNull("container", container);

            Container = container;
            BlobName = blobName;
        }
    }
}