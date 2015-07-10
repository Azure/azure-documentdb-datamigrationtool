using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;

namespace Microsoft.DataTransfer.RavenDb.Wpf.Source
{
    /// <summary>
    /// Provides configuration for RavenDB data source.
    /// </summary>
    public sealed class RavenDbSourceAdapterConfigurationProvider : DataAdapterConfigurationProviderWrapper
    {
        /// <summary>
        /// Creates a new instance of <see cref="RavenDbSourceAdapterConfigurationProvider" />.
        /// </summary>
        public RavenDbSourceAdapterConfigurationProvider()
            : base(new RavenDbSourceAdapterInternalConfigurationProvider()) { }
    }
}
