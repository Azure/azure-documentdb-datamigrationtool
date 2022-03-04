using Microsoft.DataTransfer.Basics.Files.Shared;
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
            base.Dispose(disposing);
        }
    }
}