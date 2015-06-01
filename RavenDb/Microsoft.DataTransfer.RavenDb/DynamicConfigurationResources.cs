using System;
using System.Globalization;

namespace Microsoft.DataTransfer.RavenDb
{
    /// <summary>
    /// Contains dynamic resources for data adapters configuration.
    /// </summary>
    public static class DynamicConfigurationResources
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

        private static string Format(string format, params object[] args)
        {
            return String.Format(CultureInfo.InvariantCulture, format, args);
        }
    }
}
