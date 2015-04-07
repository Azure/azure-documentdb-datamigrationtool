using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.DataTransfer.WpfHost.Basics.ValueConverters
{
    /// <summary>
    /// Converts values to user-friendly display names based on the provided mapping.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    public abstract class DisplayNameValueConverter<T> : ValueConverterBase<T, string>
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>User-friendly display name that represents current value.</returns>
        protected override string Convert(T value, object parameter, CultureInfo culture)
        {
            string converted;
            if (!GetDisplayNames().TryGetValue(value, out converted))
                converted = ConvertFromUnknownValue(value);
            return converted;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The display name produced by the binding target.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Converted value that corresponds to current display name.</returns>
        protected override T ConvertBack(string value, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return default(T);

            foreach (var mapping in GetDisplayNames())
                if (value.Equals(mapping.Value, StringComparison.Ordinal))
                    return mapping.Key;

            return ConvertFromUnknownDisplayName(value);
        }

        /// <summary>
        /// Provides mapping between known values and their user-friendly representations.
        /// </summary>
        /// <returns>Display names mapping for all known values.</returns>
        public abstract IReadOnlyDictionary<T, string> GetDisplayNames();

        /// <summary>
        /// Retrieves user-friendly representation for an unknown value.
        /// </summary>
        /// <param name="value">Unknown value.</param>
        /// <returns>Display name.</returns>
        protected abstract string ConvertFromUnknownValue(T value);

        /// <summary>
        /// Resolves the value based on an unknown display name.
        /// </summary>
        /// <param name="displayName">Display name.</param>
        /// <returns>Resolved value.</returns>
        protected abstract T ConvertFromUnknownDisplayName(string displayName);
    }
}
