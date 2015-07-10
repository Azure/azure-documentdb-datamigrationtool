using Microsoft.DataTransfer.WpfHost.Basics.Commands;
using System.Windows.Controls;
using System.Windows.Input;

namespace Microsoft.DataTransfer.WpfHost.Basics.Commands
{
    /// <summary>
    /// Base class for view model commands to replace data in the focused text box. 
    /// </summary>
    public abstract class ReplaceSelectionInFocusedTextBoxCommandBase : CommandBase
    {
        /// <summary>
        /// Determines whether or not the data can be repalced in the focused text box.
        /// </summary>
        /// <param name="parameter">The data to paste.</param>
        /// <returns>true if the data can be replaced in the text box; otherwise, false.</returns>
        public sealed override bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Replaces the data in the focused text box.
        /// </summary>
        /// <param name="parameter">Arbitrary command parameter.</param>
        public sealed override void Execute(object parameter)
        {
            var textBox = Keyboard.FocusedElement as TextBox;
            if (textBox == null)
                return;

            var newText = GetText(textBox, parameter);
            if (newText == null)
                return;

            textBox.SelectedText = newText;

            var caretIndex = textBox.SelectionStart + textBox.SelectionLength;
            textBox.Select(0, 0);
            textBox.CaretIndex = caretIndex;
        }

        /// <summary>
        /// Returns the text to use as a replacement.
        /// </summary>
        /// <param name="textBox">Target text box.</param>
        /// <param name="parameter">Arbitrary command parameter.</param>
        /// <returns>Text to use as a replacement.</returns>
        protected abstract string GetText(TextBox textBox, object parameter);
    }
}
