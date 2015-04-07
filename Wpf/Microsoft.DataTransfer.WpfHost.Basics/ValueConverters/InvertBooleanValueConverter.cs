
namespace Microsoft.DataTransfer.WpfHost.Basics.ValueConverters
{
    /// <summary>
    /// Inverts boolean value.
    /// </summary>
    public sealed class InvertBooleanValueConverter : BooleanValueConverter<bool>
    {
        /// <summary>
        /// Creates a new instance of <see cref="InvertBooleanValueConverter" />.
        /// </summary>
        public InvertBooleanValueConverter()
            : base(false, true) { }
    }
}
