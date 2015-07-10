using Microsoft.DataTransfer.WpfHost.Basics.Commands;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.HBase.Wpf.Source.Commands
{
    sealed class ReplaceWithBase64StringCommand : ReplaceSelectionInFocusedTextBoxCommandBase
    {
        protected override string GetText(TextBox textBox, object parameter)
        {
            var dialog = new ReadStringDialog
            {
                Owner = Application.Current.MainWindow
            };

            if (dialog.ShowDialog() != true)
                return null;

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(dialog.InputString));
        }
    }
}
