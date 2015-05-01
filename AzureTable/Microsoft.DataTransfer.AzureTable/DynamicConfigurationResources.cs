using Microsoft.DataTransfer.AzureTable.Source;
using System;
using System.Globalization;

namespace Microsoft.DataTransfer.AzureTable
{
    /// <summary>
    /// Contains dynamic resources for data adapters configuration.
    /// </summary>
    public static class DynamicConfigurationResources
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
        
        private static string Format(string format, params object[] args)
        {
            return String.Format(CultureInfo.InvariantCulture, format, args);
        }
    }
}
