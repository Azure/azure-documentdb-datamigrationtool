using Microsoft.DataTransfer.Basics;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Microsoft.DataTransfer.WpfHost.Basics.ValueConverters
{
    /// <summary>
    /// Provides basic functionality to convert values.
    /// </summary>
    /// <typeparam name="TSource">Type of the source value.</typeparam>
    /// <typeparam name="TTarget">Type of the target value.</typeparam>
    public abstract class ValueConverterBase<TSource, TTarget> : IValueConverter
    {
        private static Type TSourceType = typeof(TSource);
        private static Type TTargetType = typeof(TTarget);

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Guard.NotNull("targetType", targetType);

            if (!targetType.IsAssignableFrom(TTargetType))
                throw Errors.InvalidTargetConvertionType(targetType, TTargetType);

            var valueType = value == null ? typeof(object) : value.GetType();
            if (!TSourceType.IsAssignableFrom(valueType) && !(value == null && TSourceType.IsClass))
                throw Errors.InvalidSourceConvertionType(valueType, TSourceType);

            return Convert((TSource)value, parameter, culture);
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        protected abstract TTarget Convert(TSource value, object parameter, CultureInfo culture);

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Guard.NotNull("targetType", targetType);

            if (!targetType.IsAssignableFrom(TSourceType))
                throw Errors.InvalidTargetConvertionType(targetType, TSourceType);

            var valueType = value == null ? typeof(object) : value.GetType();
            if (!TTargetType.IsAssignableFrom(valueType) && !(value == null && TTargetType.IsClass))
                throw Errors.InvalidSourceConvertionType(valueType, TTargetType);

            return ConvertBack((TTarget)value, parameter, culture);
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        protected abstract TSource ConvertBack(TTarget value, object parameter, CultureInfo culture);
    }
}
