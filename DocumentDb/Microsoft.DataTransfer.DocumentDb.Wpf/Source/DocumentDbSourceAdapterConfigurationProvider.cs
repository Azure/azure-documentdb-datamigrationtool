using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Source
{
    /// <summary>
    /// Provides configuration for DocumentDB data source.
    /// </summary>
    public sealed class DocumentDbSourceAdapterConfigurationProvider : DataAdapterConfigurationProviderWrapper
    {
        /// <summary>
        /// Creates a new instance of <see cref="DocumentDbSourceAdapterConfigurationProvider" />.
        /// </summary>
        public DocumentDbSourceAdapterConfigurationProvider()
            : base(new DocumentDbSourceAdapterInternalConfigurationProvider()) { }
    }
}
