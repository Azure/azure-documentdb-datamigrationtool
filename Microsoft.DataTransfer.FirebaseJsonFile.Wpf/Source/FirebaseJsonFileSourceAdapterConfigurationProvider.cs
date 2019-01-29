using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;

namespace Microsoft.DataTransfer.FirebaseJsonFile.Wpf.Source
{
    /// <summary>
    /// Provides configuration for Firebase JSON files data source.
    /// </summary>
    class FirebaseJsonFileSourceAdapterConfigurationProvider : DataAdapterConfigurationProviderWrapper
    {
        /// <summary>
        /// Creates a new instance of <see cref="FirebaseJsonFileSourceAdapterConfigurationProvider" />.
        /// </summary>
        public FirebaseJsonFileSourceAdapterConfigurationProvider()
            : base(new FirebaseJsonFileSourceAdapterInternalConfigurationProvider()) { }
    }
}
