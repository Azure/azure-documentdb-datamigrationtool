using System;
using System.Globalization;

namespace Microsoft.DataTransfer.Basics
{
    /// <summary>
    /// Provides base functionality for dynamic resources.
    /// </summary>
    public abstract class DynamicResourcesBase
    {
        /// <summary>
        /// Formats the string resource using <see cref="CultureInfo.InvariantCulture" /> culture.
        /// </summary>
        /// <param name="format">String format.</param>
        /// <param name="args">Format arguments.</param>
        /// <returns>Formatted string resource.</returns>
        protected static string Format(string format, params object[] args)
        {
            return String.Format(CultureInfo.InvariantCulture, format, args);
        }
    }
}
