using Microsoft.DataTransfer.AzureTable.Shared;
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
        /// Gets the description for location mode configuration property.
        /// </summary>
        public static string LocationMode
        {
            get
            {
                return Format(ConfigurationResources.LocationModeFormat, Defaults.Current.LocationMode,
                    String.Join(", ", Enum.GetNames(typeof(AzureStorageLocationMode))));
            }
        }

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

        private DynamicConfigurationResources() { }
    }
}
