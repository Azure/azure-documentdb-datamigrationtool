using System;
using System.IO;

namespace Microsoft.DataTransfer.Basics.Files
{
    sealed class Errors : CommonErrors
    {
        private Errors() { }

        public static Exception UnknownSourceStream(string streamId)
        {
            return new InvalidOperationException(FormatMessage(Resources.UnknownSourceStreamFormat, streamId));
        }

        public static Exception UnknownSinkStream(string streamId)
        {
            return new InvalidOperationException(FormatMessage(Resources.UnknownSinkStreamFormat, streamId));
        }

        public static Exception InvalidBlobUrl()
        {
            return new UriFormatException(Resources.InvalidBlobUrl);
        }

        public static Exception BlobAlreadyExists(string blobName)
        {
            return new IOException(FormatMessage(Resources.BlobAlreadyExistsFormat, blobName));
        }
    }
}
