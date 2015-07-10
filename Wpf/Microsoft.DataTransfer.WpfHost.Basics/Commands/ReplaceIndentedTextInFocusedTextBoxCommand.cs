using System;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.WpfHost.Basics.Commands
{
    /// <summary>
    /// View model command to replace selected indented text in the focused text box. 
    /// </summary>
    public sealed class ReplaceIndentedTextInFocusedTextBoxCommand : ReplaceSelectionInFocusedTextBoxCommandBase
    {
        /// <summary>
        /// Returns the text to use as a replacement.
        /// </summary>
        /// <param name="textBox">Target text box.</param>
        /// <param name="parameter">Text data to insert.</param>
        /// <returns>Indented text to use as a replacement.</returns>
        protected override string GetText(TextBox textBox, object parameter)
        {
            return parameter == null ? null
                : ApplyIndentation(parameter.ToString(),
                    GetIndentation(textBox.GetLineText(textBox.GetLineIndexFromCharacterIndex(textBox.CaretIndex))));
        }

        private static string ApplyIndentation(string input, int indentationCount)
        {
            var indentation = new String(' ', indentationCount);
            return String.Join(Environment.NewLine + indentation, input.Replace("\r", "").Split('\n'));
        }

        private static int GetIndentation(string line)
        {
            int index = 0;
            for (; index < line.Length && line[index] == ' '; ++index) ;
            return index;
        }
    }
}
