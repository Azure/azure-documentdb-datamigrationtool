using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Basics.Files.Sink
{
    sealed class GZipSinkStreamProvider : ISinkStreamProvider
    {
        private readonly ISinkStreamProvider innerStreamProvider;

        public GZipSinkStreamProvider(ISinkStreamProvider innerStreamProvider)
        {
            Guard.NotNull("innerStreamProvider", innerStreamProvider);

            this.innerStreamProvider = innerStreamProvider;
        }

        public async Task<Stream> CreateStream(CancellationToken cancellation)
        {
            return new GZipStream(
                await innerStreamProvider.CreateStream(cancellation),
                CompressionMode.Compress);
        }
    }
}
