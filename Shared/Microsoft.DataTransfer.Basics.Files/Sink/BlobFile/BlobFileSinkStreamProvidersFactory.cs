using Microsoft.DataTransfer.Basics.Files.Shared;

namespace Microsoft.DataTransfer.Basics.Files.Sink.BlobFile
{
    sealed class BlobFileSinkStreamProvidersFactory : BlobFileStreamProvidersFactoryBase, ISinkStreamProvidersFactory
    {
        public ISinkStreamProvider Create(string streamId, bool overwrite)
        {
            if (!IsBlobAddress(streamId))
                return null;

            var blobReference = GetBlobReference(streamId);

            return new BlobFileSinkStreamProvider(
                blobReference.Container.GetBlockBlobReference(blobReference.BlobName), overwrite);
        }
    }
}
