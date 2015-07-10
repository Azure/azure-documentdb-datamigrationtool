using Microsoft.DataTransfer.Basics.Files.Shared;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.DataTransfer.Basics.Files.Source.BlobFile
{
    sealed class BlobFileSourceStreamProvidersFactory : BlobFileStreamProvidersFactoryBase, ISourceStreamProvidersFactory
    {
        public IEnumerable<ISourceStreamProvider> Create(string streamId)
        {
            if (!IsBlobAddress(streamId))
                return null;

            var blobReference = GetBlobReference(streamId);

            return FilterBlobs(
                blobReference.Container.ListBlobs(String.Empty, true),
                new Regex(blobReference.BlobName, RegexOptions.Compiled));
        }

        private static IEnumerable<ISourceStreamProvider> FilterBlobs(IEnumerable<IListBlobItem> blobs, Regex filterRegex)
        {
            foreach (var listItem in blobs)
            {
                var blob = listItem as ICloudBlob;

                if (blob == null)
                    continue;

                var match = filterRegex.Match(blob.Name);
                // Make sure regex matches entire BLOB name and not just substring
                if (!match.Success || !String.Equals(match.Value, blob.Name, StringComparison.InvariantCulture))
                    continue;

                yield return new BlobFileSourceStreamProvider(blob);
            }
        }
    }
}
