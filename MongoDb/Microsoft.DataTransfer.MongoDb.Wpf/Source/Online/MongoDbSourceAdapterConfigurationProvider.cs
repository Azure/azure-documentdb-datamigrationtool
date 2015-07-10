using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;

namespace Microsoft.DataTransfer.MongoDb.Wpf.Source.Online
{
    /// <summary>
    /// Provides configuration for MongoDB data source.
    /// </summary>
    public sealed class MongoDbSourceAdapterConfigurationProvider : DataAdapterConfigurationProviderWrapper
    {
        /// <summary>
        /// Creates a new instance of <see cref="MongoDbSourceAdapterConfigurationProvider" />.
        /// </summary>
        public MongoDbSourceAdapterConfigurationProvider()
            : base(new MongoDbSourceAdapterInternalConfigurationProvider()) { }
    }
}
