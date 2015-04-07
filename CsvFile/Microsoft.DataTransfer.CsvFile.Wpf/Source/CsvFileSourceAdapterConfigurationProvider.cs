using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;

namespace Microsoft.DataTransfer.CsvFile.Wpf.Source
{
    /// <summary>
    /// Provides configuration for CSV files data source.
    /// </summary>
    public sealed class CsvFileSourceAdapterConfigurationProvider : DataAdapterConfigurationProviderWrapper
    {
        /// <summary>
        /// Creates a new instance of <see cref="CsvFileSourceAdapterConfigurationProvider" />.
        /// </summary>
        public CsvFileSourceAdapterConfigurationProvider()
            : base(new CsvFileSourceAdapterInternalConfigurationProvider()) { }
    }
}
