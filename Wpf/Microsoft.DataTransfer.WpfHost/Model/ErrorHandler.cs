using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using System;
using System.Globalization;
using System.Windows;

namespace Microsoft.DataTransfer.WpfHost.Model
{
    sealed class ErrorHandler : IErrorHandler
    {
        public void Handle(Exception error)
        {
            MessageBox.Show(
                Application.Current.MainWindow,
                String.Format(CultureInfo.CurrentUICulture, Resources.CriticalErrorFormat, 
                    error == null ? CommonResources.UnknownError : error.Message),
                Resources.CriticalErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
