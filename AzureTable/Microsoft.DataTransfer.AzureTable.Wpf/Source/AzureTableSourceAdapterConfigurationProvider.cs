using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;

namespace Microsoft.DataTransfer.AzureTable.Wpf.Source
{
    /// <summary>
    /// Provides configuration for Azure Table storage data source.
    /// </summary>
    public sealed class AzureTableSourceAdapterConfigurationProvider : DataAdapterConfigurationProviderWrapper
    {
        /// <summary>
        /// Creates a new instance of <see cref="AzureTableSourceAdapterConfigurationProvider" />.
        /// </summary>
        public AzureTableSourceAdapterConfigurationProvider()
            : base(new AzureTableSourceAdapterInternalConfigurationProvider()) { }
    }
}
