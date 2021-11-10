using Azure.Storage.Blobs;
using Microsoft.DataTransfer.Basics.Files.Shared;
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
                blobReference.Container,
                new Regex(blobReference.BlobName, RegexOptions.Compiled));
        }

        private static IEnumerable<ISourceStreamProvider> FilterBlobs(BlobContainerClient client, Regex filterRegex)
        {          
            foreach (var listItem in client.GetBlobs())
            {
                var blob = listItem;

                if (blob == null)
                    continue;

                var match = filterRegex.Match(blob.Name);
                // Make sure regex matches entire BLOB name and not just substring
                if (!match.Success || !String.Equals(match.Value, blob.Name, StringComparison.InvariantCulture))
                    continue;

                yield return new BlobFileSourceStreamProvider(client.GetBlobClient(blob.Name));
            }
        }
    }
}
