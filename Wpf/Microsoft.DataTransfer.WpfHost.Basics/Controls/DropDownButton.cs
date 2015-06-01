using Microsoft.DataTransfer.Basics.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls
{
    /// <summary>
    /// Button that allows users to select different actions.
    /// </summary>
    public sealed class DropDownButton : ToggleButton
    {
        private static readonly string DropDownPropertyName = ObjectExtensions.MemberName<DropDownButton>(b => b.DropDown);
        private static readonly string ContextMenuIsOpenPropertyName = ObjectExtensions.MemberName<ContextMenu>(c => c.IsOpen);

        /// <summary>
        /// Identifies the <see cref="DropDownButton.DropDown" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty DropDownProperty =
            DependencyProperty.Register(DropDownPropertyName, typeof(ContextMenu), typeof(DropDownButton));

        /// <summary>
        /// Creates a new instance of <see cref="DropDownButton" />.
        /// </summary>
        public DropDownButton()
        {
            SetBinding(IsCheckedProperty, new Binding(DropDownPropertyName + "." + ContextMenuIsOpenPropertyName) { Source = this });
        }

        /// <summary>
        /// Gets or sets context menu with all possible actions.
        /// </summary>
        public ContextMenu DropDown
        {
            get { return (ContextMenu)GetValue(DropDownProperty); }
            set { SetValue(DropDownProperty, value); }
        }

        /// <summary>
        /// Called when a control is clicked by the mouse or the keyboard.
        /// </summary>
        protected override void OnClick()
        {
            if (DropDown == null)
                return;

            DropDown.PlacementTarget = this;
            DropDown.Placement = PlacementMode.Bottom;
            DropDown.IsOpen = true;
        }
    }
}
