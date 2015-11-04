using Microsoft.DataTransfer.WpfHost.Extensibility;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Sink.Parallel
{
    /// <summary>
    /// Provides configuration for parallel DocumentDB data sink.
    /// </summary>
    public sealed class DocumentDbParallelSinkAdapterConfigurationProvider : DataAdapterConfigurationProviderWrapper
    {
        /// <summary>
        /// Creates a new instance of <see cref="DocumentDbParallelSinkAdapterConfigurationProvider" />.
        /// </summary>
        /// <param name="sharedStorage">Storage to share some configuration values for current import.</param>
        public DocumentDbParallelSinkAdapterConfigurationProvider(IImportSharedStorage sharedStorage)
            : base(new DocumentDbParallelSinkAdapterInternalConfigurationProvider(sharedStorage)) { }
    }
}
