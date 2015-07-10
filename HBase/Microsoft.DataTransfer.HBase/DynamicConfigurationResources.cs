using System;
using System.Globalization;

namespace Microsoft.DataTransfer.HBase
{
    /// <summary>
    /// Contains dynamic resources for data adapters configuration.
    /// </summary>
    public static class DynamicConfigurationResources
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

        private static string Format(string format, params object[] args)
        {
            return String.Format(CultureInfo.InvariantCulture, format, args);
        }
    }
}
