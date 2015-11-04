using Microsoft.DataTransfer.Basics;

namespace Microsoft.DataTransfer.HBase
{
    /// <summary>
    /// Contains dynamic resources for data adapters configuration.
    /// </summary>
    public sealed class DynamicConfigurationResources : DynamicResourcesBase
    {
        /// <summary>
        /// Gets the description for source batch size configuration property.
        /// </summary>
        public static string Source_BatchSize
        {
            get
            {
                return Format(ConfigurationResources.Source_BatchSizeFormat, Defaults.Current.SourceBatchSize);
            }
        }

        private DynamicConfigurationResources() { }
    }
}
