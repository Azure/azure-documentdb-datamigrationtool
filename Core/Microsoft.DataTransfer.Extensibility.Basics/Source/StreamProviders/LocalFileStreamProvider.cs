using System.IO;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Extensibility.Basics.Source.StreamProviders
{
    sealed class LocalFileStreamProvider : ISourceStreamProvider
    {
        public string Id { get; private set; }

        public LocalFileStreamProvider(string fileName)
        {
            Id = fileName;
        }

        public Task<StreamReader> CreateReader()
        {
            return Task.FromResult(File.OpenText(Id));
        }
    }
}
