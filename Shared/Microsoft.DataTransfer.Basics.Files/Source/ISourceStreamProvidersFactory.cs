using System.Collections.Generic;

namespace Microsoft.DataTransfer.Basics.Files.Source
{
    interface ISourceStreamProvidersFactory
    {
        IEnumerable<ISourceStreamProvider> Create(string streamId);
    }
}
