using Microsoft.DataTransfer.Basics.Files.Source.BlobFile;
using Microsoft.DataTransfer.Basics.Files.Source.LocalFile;
using Microsoft.DataTransfer.Basics.Files.Source.WebFile;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.Basics.Files.Source
{
    /// <summary>
    /// Creates instances of <see cref="ISourceStreamProvider" /> based on the source stream identifier.
    /// </summary>
    public static class SourceStreamProvidersFactory
    {
        private static readonly ISourceStreamProvidersFactory[] factories = new ISourceStreamProvidersFactory[]
        {
            // NOTE: Order matters! LocalFile will create provider for any string
            new BlobFileSourceStreamProvidersFactory(),
            new WebFileSourceStreamProvidersFactory(),
            new LocalFileSourceStreamProvidersFactory()
        };

        /// <summary>
        /// Creates a new instances of <see cref="ISourceStreamProvider" /> for the specified <paramref name="streamId" />.
        /// </summary>
        /// <param name="streamId">Identifier of the source stream.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> of <see cref="ISourceStreamProvider" /> to read data from the specified source stream.</returns>
        public static IEnumerable<ISourceStreamProvider> Create(string streamId)
        {
            foreach (var factory in factories)
            {
                var providers = factory.Create(streamId);
                if (providers != null)
                    return providers;
            }

            throw Errors.UnknownSourceStream(streamId);
        }
    }
}
