using Microsoft.DataTransfer.Basics.Files.Shared;
using Microsoft.DataTransfer.Basics.IO;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.Basics.Files.Source.LocalFile
{
    sealed class LocalFileSourceStreamProvidersFactory : LocalFileStreamProvidersFactoryBase, ISourceStreamProvidersFactory
    {
        public IEnumerable<ISourceStreamProvider> Create(string streamId)
        {
            foreach (var localFile in DirectoryHelper.EnumerateFiles(TrimUriFormat(streamId)))
                yield return new LocalFileSourceStreamProvider(localFile);
        }
    }
}
