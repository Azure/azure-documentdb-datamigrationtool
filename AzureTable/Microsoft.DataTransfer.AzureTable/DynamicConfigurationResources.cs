using Microsoft.DataTransfer.AzureTable.Source;
using Microsoft.DataTransfer.Basics;
using System;

namespace Microsoft.DataTransfer.AzureTable
{
    /// <summary>
    /// Contains dynamic resources for data adapters configuration.
    /// </summary>
    public sealed class DynamicConfigurationResources : DynamicResourcesBase
    {
        /// <summary>
        /// Gets the description for source internal fields configuration property.
        /// </summary>
        public static string Source_InternalFields
        {
            get
            {
                return Format(ConfigurationResources.Source_InternalFieldsFormat, Defaults.Current.SourceInternalFields,
                    String.Join(", ", Enum.GetNames(typeof(AzureTableInternalFields))));
            }
        }

        /// <summary>
        /// Gets the description for source internal fields configuration property.
        /// </summary>
        public static string Source_LocationMode
        {
            get
            {
                return Format(ConfigurationResources.Source_LocationMode, Defaults.Current.SourceLocationMode,
                    String.Join(", ", Enum.GetNames(typeof(AzureTableLocationMode))));
            }
        }
        
        private DynamicConfigurationResources() { }
    }
}
