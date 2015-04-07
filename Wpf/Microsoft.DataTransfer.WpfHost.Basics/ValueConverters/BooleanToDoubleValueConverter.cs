
namespace Microsoft.DataTransfer.WpfHost.Basics.ValueConverters
{
    /// <summary>
    /// Converts boolean value to double.
    /// </summary>
    public sealed class BooleanToDoubleValueConverter : BooleanValueConverter<double>
    {
        /// <summary>
        /// Creates a new instance of <see cref="BooleanToDoubleValueConverter" />.
        /// </summary>
        public BooleanToDoubleValueConverter()
            : base(1, 0) { }
    }
}
