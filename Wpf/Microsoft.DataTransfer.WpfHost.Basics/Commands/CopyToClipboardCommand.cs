using System;
using System.Windows;

namespace Microsoft.DataTransfer.WpfHost.Basics.Commands
{
    /// <summary>
    /// View model command to copy data to clipboard. 
    /// </summary>
    public sealed class CopyToClipboardCommand : CommandBase
    {
        /// <summary>
        /// Determines whether or not the data can be copied to the clipboard.
        /// </summary>
        /// <param name="parameter">The data to be copied.</param>
        /// <returns>true if the data can be copied to the clipboard; otherwise, false.</returns>
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Copies the data to the clipboard.
        /// </summary>
        /// <param name="parameter">The data to be copied.</param>
        public override void Execute(object parameter)
        {
            Clipboard.SetText(parameter == null ? String.Empty : parameter.ToString());
        }
    }
}
