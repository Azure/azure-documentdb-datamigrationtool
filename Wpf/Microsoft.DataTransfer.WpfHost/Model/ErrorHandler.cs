using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace Microsoft.DataTransfer.WpfHost.Model
{
    sealed class ErrorHandler : IErrorHandler
    {
        public void Handle(Exception error)
        {
            MessageBox.Show(
                Application.Current.MainWindow,
                String.Format(CultureInfo.CurrentUICulture, Resources.CriticalErrorFormat, GetErrorMessage(error)),
                Resources.CriticalErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private static string GetErrorMessage(Exception error)
        {
            if (error == null)
                return CommonResources.UnknownError;

            if (error is AggregateException)
                return String.Join(Environment.NewLine, 
                    ((AggregateException)error).Flatten().InnerExceptions.Select(e => e.Message));

            return error.Message;
        }
    }
}
