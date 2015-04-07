using Microsoft.DataTransfer.WpfHost.Extensibility;

namespace Microsoft.DataTransfer.WpfHost.ServiceModel
{
    /// <summary>
    /// Collection of data adapter configuration providers.
    /// </summary>
    public interface IDataAdapterConfigurationProvidersCollection
    {
        /// <summary>
        /// Retrieves configuration provider for specified data source adapter.
        /// </summary>
        /// <param name="source">Name of the data source adapter.</param>
        /// <returns>Configuration provider or null, if no providers found for the specified data source adapter.</returns>
        IDataAdapterConfigurationProvider GetForSource(string source);

        /// <summary>
        /// Retrieves configuration provider for specified data sink adapter.
        /// </summary>
        /// <param name="sink">Name of the data sink adapter.</param>
        /// <returns>Configuration provider or null, if no providers found for the specified data sink adapter.</returns>
        IDataAdapterConfigurationProvider GetForSink(string sink);
    }
}
