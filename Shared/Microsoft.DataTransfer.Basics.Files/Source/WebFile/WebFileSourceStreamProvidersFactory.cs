using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.DataTransfer.Basics.Files.Source.WebFile
{
    sealed class WebFileSourceStreamProvidersFactory : ISourceStreamProvidersFactory
    {
        private readonly static Regex WebAddressThumbprintRegex = new Regex("^https?://", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public IEnumerable<ISourceStreamProvider> Create(string streamId)
        {
            if (!WebAddressThumbprintRegex.IsMatch(streamId))
                return null;

            return new[] { new WebFileSourceStreamProvider(streamId) };
        }
    }
}
