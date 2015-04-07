using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;

namespace Microsoft.DataTransfer.JsonFile.Wpf.Source
{
    /// <summary>
    /// Provides configuration for JSON files data source.
    /// </summary>
    public sealed class JsonFileSourceAdapterConfigurationProvider : DataAdapterConfigurationProviderWrapper
    {
        /// <summary>
        /// Creates a new instance of <see cref="JsonFileSourceAdapterConfigurationProvider" />.
        /// </summary>
        public JsonFileSourceAdapterConfigurationProvider()
            : base(new JsonFileSourceAdapterInternalConfigurationProvider()) { }
    }
}
