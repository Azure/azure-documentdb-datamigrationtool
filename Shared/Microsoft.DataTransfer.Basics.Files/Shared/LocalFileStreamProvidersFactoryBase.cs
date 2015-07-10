using System;

namespace Microsoft.DataTransfer.Basics.Files.Shared
{
    abstract class LocalFileStreamProvidersFactoryBase
    {
        private readonly static string FileAddressPrefix = "file:///";

        protected static string TrimUriFormat(string localFile)
        {
            Guard.NotNull("localFile", localFile);

            return localFile.StartsWith(FileAddressPrefix, StringComparison.OrdinalIgnoreCase)
                ? localFile.Substring(FileAddressPrefix.Length) : localFile;
        }
    }
}
