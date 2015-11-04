using Microsoft.DataTransfer.WpfHost.Basics.Dialogs;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using System;
using System.Windows;

namespace Microsoft.DataTransfer.WpfHost.Model
{
    sealed class ErrorHandler : IErrorHandler
    {
        public void Handle(Exception error)
        {
            ExceptionMessageBox.Show(Application.Current.MainWindow, Resources.CriticalErrorCaption, error);
        }
    }
}
