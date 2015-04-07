using System;
using System.Globalization;
using System.Windows;

namespace Microsoft.DataTransfer.WpfHost.Basics.ValueConverters
{
    /// <summary>
    /// Provides element visibility value depending on string content.
    /// </summary>
    public sealed class StringNotEmptyToVisibilityValueConverter : ValueConverterBase<string, Visibility>
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns><see cref="Visibility.Visible" /> if <paramref name="value" /> is not empty; otherwise, <see cref="Visibility.Collapsed" />.</returns>
        protected override Visibility Convert(string value, object parameter, CultureInfo culture)
        {
            return String.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Visible string if <paramref name="value" /> is <see cref="Visibility.Visible"/>; otherwise, empty string.</returns>
        protected override string ConvertBack(Visibility value, object parameter, CultureInfo culture)
        {
            return value == Visibility.Visible ? value.ToString() : String.Empty;
        }
    }
}
