using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;

namespace Microsoft.DataTransfer.MongoDb.Wpf.Source.Mongoexport
{
    /// <summary>
    /// Provides configuration for mongoexport JSON files data source.
    /// </summary>
    public sealed class MongoexportFileSourceAdapterConfigurationProvider: DataAdapterConfigurationProviderWrapper
    {
        /// <summary>
        /// Creates a new instance of <see cref="MongoexportFileSourceAdapterConfigurationProvider" />.
        /// </summary>
        public MongoexportFileSourceAdapterConfigurationProvider()
            : base(new MongoexportFileSourceAdapterInternalConfigurationProvider()) { }
    }
}
