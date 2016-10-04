using Microsoft.DataTransfer.Basics.Files.Sink.BlobFile;
using Microsoft.DataTransfer.Basics.Files.Sink.LocalFile;

namespace Microsoft.DataTransfer.Basics.Files.Sink
{
    /// <summary>
    /// Creates instances of <see cref="ISinkStreamProvider" /> based on the sink stream identifier.
    /// </summary>
    public class SinkStreamProvidersFactory
    {
        private static readonly ISinkStreamProvidersFactory[] factories = new ISinkStreamProvidersFactory[]
        {
            // NOTE: Order matters! LocalFile will create provider for any string
            new BlobFileSinkStreamProvidersFactory(),
            new LocalFileSinkStreamProvidersFactory()
        };

        /// <summary>
        /// Creates a new instance of <see cref="ISinkStreamProvider" /> for the specified <paramref name="streamId" />.
        /// </summary>
        /// <param name="streamId">Identifier of the target stream.</param>
        /// <param name="compress">Whether the data in the target stream should be compressed.</param>
        /// <param name="overwrite">Whether underlying storage stream can be replaced.</param>
        /// <returns>A <see cref="ISinkStreamProvider" /> to write data to the specified target stream.</returns>
        public static ISinkStreamProvider Create(string streamId, bool compress, bool overwrite)
        {
            foreach (var factory in factories)
            {
                var provider = factory.Create(streamId, overwrite);
                if (provider != null)
                {
                    if (compress)
                        provider = new GZipSinkStreamProvider(provider);

                    return provider;
                }
            }

            throw Errors.UnknownSinkStream(streamId);
        }
    }
}
