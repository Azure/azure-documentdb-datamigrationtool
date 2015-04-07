using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Sink.Bulk
{
    /// <summary>
    /// Provides configuration for bulk DocumentDB data sink.
    /// </summary>
    public sealed class DocumentDbBulkSinkAdapterConfigurationProvider : DataAdapterConfigurationProviderWrapper
    {
        /// <summary>
        /// Creates a new instance of <see cref="DocumentDbBulkSinkAdapterConfigurationProvider" />.
        /// </summary>
        public DocumentDbBulkSinkAdapterConfigurationProvider()
            : base(new DocumentDbBulkSinkAdapterInternalConfigurationProvider()) { }
    }
}
