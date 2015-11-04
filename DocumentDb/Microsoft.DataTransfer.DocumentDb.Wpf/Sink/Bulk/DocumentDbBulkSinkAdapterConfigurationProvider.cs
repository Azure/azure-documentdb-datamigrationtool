using Microsoft.DataTransfer.WpfHost.Extensibility;
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
        /// <param name="sharedStorage">Storage to share some configuration values for current import.</param>
        public DocumentDbBulkSinkAdapterConfigurationProvider(IImportSharedStorage sharedStorage)
            : base(new DocumentDbBulkSinkAdapterInternalConfigurationProvider(sharedStorage)) { }
    }
}
