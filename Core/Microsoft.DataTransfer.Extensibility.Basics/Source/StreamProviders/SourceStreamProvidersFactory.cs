using Microsoft.DataTransfer.Basics.IO;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.DataTransfer.Extensibility.Basics.Source.StreamProviders
{
    /// <summary>
    /// Creates instances of <see cref="ISourceStreamProvider" /> based on the source stream identifier.
    /// </summary>
    public static class SourceStreamProvidersFactory
    {
        private readonly static Regex WebAddressRegex = new Regex("^https?://", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Creates new instances of <see cref="ISourceStreamProvider" /> for the specified <paramref name="sourceStreamId" />.
        /// </summary>
        /// <param name="sourceStreamId">Identifier of the source stream.</param>
        /// <returns><see cref="IEnumerable{T}" /> of <see cref="ISourceStreamProvider" /> to read data from the specified source stream.</returns>
        public static IEnumerable<ISourceStreamProvider> Create(string sourceStreamId)
        {
            if (WebAddressRegex.IsMatch(sourceStreamId))
            {
                yield return new WebFileStreamProvider(sourceStreamId);
            }
            else
            {
                foreach (var localFile in DirectoryHelper.EnumerateFiles(TrimUriFormat(sourceStreamId)))
                    yield return new LocalFileStreamProvider(localFile);
            }
        }

        private static string TrimUriFormat(string localFile)
        {
            return localFile.StartsWith("file:///", StringComparison.OrdinalIgnoreCase) ? localFile.Substring(8) : localFile;
        }
    }
}
