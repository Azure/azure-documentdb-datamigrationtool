using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using System.IO;

namespace Microsoft.DataTransfer.Basics.Files.Sink.BlobFile
{
    sealed class BlobStreamWriter : StreamWriter
    {
        private readonly string blobUrl;

        public BlobStreamWriter(Stream stream, string blobUrl)
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
