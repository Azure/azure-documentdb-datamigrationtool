using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Basics.Files.Source
{
    sealed class GZipSourceStreamProvider : ISourceStreamProvider
    {
        private readonly ISourceStreamProvider innerStreamProvider;

        public string Id
        {
            get { return innerStreamProvider.Id; }
        }

        public GZipSourceStreamProvider(ISourceStreamProvider innerStreamProvider)
        {
            Guard.NotNull("innerStreamProvider", innerStreamProvider);

            this.innerStreamProvider = innerStreamProvider;
        }

        public async Task<Stream> CreateStream(CancellationToken cancellation)
        {
            return new GZipStream(
                await innerStreamProvider.CreateStream(cancellation),
                CompressionMode.Decompress);
        }
    }
}
