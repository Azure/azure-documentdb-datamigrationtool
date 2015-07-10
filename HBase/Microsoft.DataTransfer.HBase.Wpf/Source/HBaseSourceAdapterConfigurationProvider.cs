using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;

namespace Microsoft.DataTransfer.HBase.Wpf.Source
{
    /// <summary>
    /// Provides configuration for HBase data source.
    /// </summary>
    public sealed class HBaseSourceAdapterConfigurationProvider : DataAdapterConfigurationProviderWrapper
    {
        /// <summary>
        /// Creates a new instance of <see cref="HBaseSourceAdapterConfigurationProvider" />.
        /// </summary>
        public HBaseSourceAdapterConfigurationProvider()
            : base(new HBaseSourceAdapterInternalConfigurationProvider()) { }
    }
}
