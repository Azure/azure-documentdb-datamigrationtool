using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Basics.Files.Sink.LocalFile
{
    sealed class LocalFileSinkStreamProvider : ISinkStreamProvider
    {
        private readonly string fileName;
        private readonly bool overwrite;

        public LocalFileSinkStreamProvider(string fileName, bool overwrite)
        {
            Guard.NotEmpty("fileName", fileName);

            this.fileName = fileName;
            this.overwrite = overwrite;
        }

        public Task<Stream> CreateStream(CancellationToken cancellation)
        {
            // Ensure output folder exists
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }
            catch { }

            return Task.FromResult<Stream>(
                File.Open(fileName, overwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write, FileShare.Read));
        }
    }
}
