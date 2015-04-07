
namespace Microsoft.DataTransfer.WpfHost.Basics.ValueConverters
{
    /// <summary>
    /// Converts boolean value to string.
    /// </summary>
    public sealed class BooleanToStringValueConverter : BooleanValueConverter<string>
    {
        /// <summary>
        /// Creates a new instance of <see cref="BooleanToStringValueConverter" />.
        /// </summary>
        public BooleanToStringValueConverter() 
            : base(Resources.DefaultTrueValue, Resources.DefaultFalseValue) {}
    }
}
