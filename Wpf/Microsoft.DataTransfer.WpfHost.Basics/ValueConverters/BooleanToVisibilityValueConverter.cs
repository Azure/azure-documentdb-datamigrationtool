using System.Windows;

namespace Microsoft.DataTransfer.WpfHost.Basics.ValueConverters
{
    /// <summary>
    /// Converts boolean value to visibility.
    /// </summary>
    public sealed class BooleanToVisibilityValueConverter : BooleanValueConverter<Visibility>
    {
        /// <summary>
        /// Creates a new instance of <see cref="BooleanToVisibilityValueConverter" />.
        /// </summary>
        public BooleanToVisibilityValueConverter()
            : base(Visibility.Visible, Visibility.Collapsed) { }
    }
}
