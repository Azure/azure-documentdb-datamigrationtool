using Microsoft.DataTransfer.Basics;

namespace Microsoft.DataTransfer.RavenDb
{
    /// <summary>
    /// Contains dynamic resources for data adapters configuration.
    /// </summary>
    public sealed class DynamicConfigurationResources : DynamicResourcesBase
    {
        /// <summary>
        /// Gets the description for source index name property.
        /// </summary>
        public static string Source_Index
        {
            get
            {
                return Format(ConfigurationResources.Source_IndexFormat, Defaults.Current.SourceIndex);
            }
        }

        private DynamicConfigurationResources() { }
    }
}
