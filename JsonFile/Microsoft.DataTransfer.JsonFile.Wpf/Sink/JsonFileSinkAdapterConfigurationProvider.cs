using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;

namespace Microsoft.DataTransfer.JsonFile.Wpf.Sink
{
    /// <summary>
    /// Provides configuration for JSON file data sink.
    /// </summary>
    public sealed class JsonFileSinkAdapterConfigurationProvider : DataAdapterConfigurationProviderWrapper
    {
        /// <summary>
        /// Creates a new instance of <see cref="JsonFileSinkAdapterConfigurationProvider" />.
        /// </summary>
        public JsonFileSinkAdapterConfigurationProvider()
            : base(new JsonFileSinkAdapterInternalConfigurationProvider()) { }
    }
}
