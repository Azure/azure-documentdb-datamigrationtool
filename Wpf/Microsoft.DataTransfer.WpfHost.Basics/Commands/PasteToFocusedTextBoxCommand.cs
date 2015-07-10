using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Microsoft.DataTransfer.WpfHost.Basics.Commands
{
    /// <summary>
    /// View model command to paste data into the focused text box. 
    /// </summary>
    public sealed class PasteToFocusedTextBoxCommand : CommandBase
    {
        /// <summary>
        /// Determines whether or not the data can be pasted to the focused text box.
        /// </summary>
        /// <param name="parameter">The data to paste.</param>
        /// <returns>true if the data can be pasted to the text box; otherwise, false.</returns>
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Pastes the data into the focused text box.
        /// </summary>
        /// <param name="parameter">The data to paste.</param>
        public override void Execute(object parameter)
        {
            var textBox = Keyboard.FocusedElement as TextBox;
            if (textBox == null)
                return;

            if (String.IsNullOrEmpty(textBox.Text) ||
                MessageBox.Show(Resources.ReplaceContentConfirmation, Resources.ReplaceContentConfirmationTitle,
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                textBox.Text = parameter == null ? null : parameter.ToString();
                textBox.CaretIndex = Int32.MaxValue;
            }
        }
    }
}
