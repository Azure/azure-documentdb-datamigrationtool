using System;
using System.Windows;

namespace Microsoft.DataTransfer.WpfHost.Basics.Dialogs
{
    partial class ExceptionDialog : Window
    {
        private ExceptionDialog()
        {
            InitializeComponent();
        }

        public ExceptionDialog(Exception error)
            : this()
        {
            DataContext = new ExceptionDialogViewModel(error);
        }
    }
}
