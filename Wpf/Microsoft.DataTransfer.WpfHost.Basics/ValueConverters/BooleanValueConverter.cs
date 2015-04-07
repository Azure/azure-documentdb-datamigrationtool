using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.DataTransfer.WpfHost.Basics.ValueConverters
{
    /// <summary>
    /// Provides basic functionality to convert boolean values.
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public abstract class BooleanValueConverter<TTarget> : ValueConverterBase<bool, TTarget>
    {
        /// <summary>
        /// Gets or sets result value, returned when source value is true.
        /// </summary>
        public TTarget True { get; set; }

        /// <summary>
        /// Gets or sets result value, returned when source value is false.
        /// </summary>
        public TTarget False { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="BooleanValueConverter{TTarget}" />.
        /// </summary>
        /// <param name="trueValue">Result value, returned when source value is true.</param>
        /// <param name="falseValue">Result value, returned when source value is false.</param>
        protected BooleanValueConverter(TTarget trueValue, TTarget falseValue)
        {
            True = trueValue;
            False = falseValue;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        protected override TTarget Convert(bool value, object parameter, CultureInfo culture)
        {
            return value ? True : False;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>true if provided <paramref name="value" /> equals to <see cref="BooleanValueConverter{TTarget}.True" /> property value; otherwise, false.</returns>
        protected override bool ConvertBack(TTarget value, object parameter, CultureInfo culture)
        {
            return EqualityComparer<TTarget>.Default.Equals(value, True);
        }
    }
}
