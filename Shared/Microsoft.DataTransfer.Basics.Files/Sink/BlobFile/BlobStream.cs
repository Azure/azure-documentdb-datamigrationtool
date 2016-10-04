using Microsoft.DataTransfer.Basics.Files.Shared;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using System.IO;

namespace Microsoft.DataTransfer.Basics.Files.Sink.BlobFile
{
    sealed class BlobStream : WrapperStream
    {
        private readonly string blobUrl;

        public BlobStream(Stream stream, string blobUrl)
            : base(stream)
        {
            this.blobUrl = blobUrl;
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(disposing);
            }
            catch (StorageException storageException)
            {
                if (storageException.RequestInformation != null &&
                    storageException.RequestInformation.ExtendedErrorInformation != null &&
                    storageException.RequestInformation.ExtendedErrorInformation.ErrorCode
                        == BlobErrorCodeStrings.BlobAlreadyExists)
                {
                    throw Errors.BlobAlreadyExists(blobUrl);
                }

                throw;
            }
        }
    }
}
