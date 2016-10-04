using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Basics.Files.Source.LocalFile
{
    sealed class LocalFileSourceStreamProvider : ISourceStreamProvider
    {
        public string Id { get; private set; }

        public LocalFileSourceStreamProvider(string fileName)
        {
            Id = fileName;
        }

        public Task<Stream> CreateStream(CancellationToken cancellation)
        {
            return Task.FromResult<Stream>(File.OpenRead(Id));
        }
    }
}
