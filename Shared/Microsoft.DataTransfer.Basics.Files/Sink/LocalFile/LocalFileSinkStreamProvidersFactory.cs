using Microsoft.DataTransfer.Basics.Files.Shared;

namespace Microsoft.DataTransfer.Basics.Files.Sink.LocalFile
{
    sealed class LocalFileSinkStreamProvidersFactory : LocalFileStreamProvidersFactoryBase, ISinkStreamProvidersFactory
    {
        public ISinkStreamProvider Create(string sinkStreamId, bool overwrite)
        {
            return new LocalFileSinkStreamProvider(TrimUriFormat(sinkStreamId), overwrite);
        }
    }
}
