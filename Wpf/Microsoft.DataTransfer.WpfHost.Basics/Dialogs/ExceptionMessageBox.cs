using System;
using System.Windows;

namespace Microsoft.DataTransfer.WpfHost.Basics.Dialogs
{
    /// <summary>
    /// Displays an exception message box.
    /// </summary>
    public static class ExceptionMessageBox
    {
        /// <summary>
        /// Displays a message box that has specified title bar caption and error information.
        /// </summary>
        /// <param name="owner">A <see cref="Window" /> that represents the owner window of the message box.</param>
        /// <param name="caption">A <see cref="string" /> that specifies the title bar caption to display.</param>
        /// <param name="error">An <see cref="Exception" /> that specifies the error to display.</param>
        public static void Show(Window owner, string caption, Exception error)
        {
            new ExceptionDialog(error)
            {
                Owner = owner,
                Title = caption
            }.ShowDialog();
        }
    }
}
