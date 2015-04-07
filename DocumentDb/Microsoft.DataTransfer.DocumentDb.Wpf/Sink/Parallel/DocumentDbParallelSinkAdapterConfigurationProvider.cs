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
        public DocumentDbParallelSinkAdapterConfigurationProvider()
            : base(new DocumentDbParallelSinkAdapterInternalConfigurationProvider()) { }
    }
}
