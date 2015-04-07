
namespace Microsoft.DataTransfer.WpfHost.Basics.ValueConverters
{
    /// <summary>
    /// Converts provided value to string.
    /// </summary>
    public sealed class ToStringValueConverter : ValueConverterBase<object, string>
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        protected override string Convert(object value, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null ? null : value.ToString();
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Provided <paramref name="value" />.</returns>
        protected override object ConvertBack(string value, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
