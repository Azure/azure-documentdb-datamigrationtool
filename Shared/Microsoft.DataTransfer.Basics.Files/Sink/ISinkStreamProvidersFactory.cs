
namespace Microsoft.DataTransfer.Basics.Files.Sink
{
    interface ISinkStreamProvidersFactory
    {
        ISinkStreamProvider Create(string streamId, bool overwrite);
    }
}
