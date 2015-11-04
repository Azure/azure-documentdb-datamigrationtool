using Microsoft.DataTransfer.Basics;
using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Microsoft.DataTransfer.WpfHost.Basics.Dialogs
{
    sealed class ExceptionDialogViewModel : BindableBase
    {
        private static readonly ImageSource ErrorIcon = 
            Imaging.CreateBitmapSourceFromHIcon(SystemIcons.Error.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        private readonly Exception error;
        private bool detailsVisible;

        public ImageSource Icon
        {
            get { return ErrorIcon; }
        }

        public string Message
        {
            get { return GetErrorMessage(error); }
        }

        public string Details
        {
            get { return GetErrorDetails(error); }
        }

        public bool DetailsVisible
        {
            get { return detailsVisible; }
            set { SetProperty(ref detailsVisible, value); }
        }

        public ExceptionDialogViewModel(Exception error)
        {
            this.error = error;
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

        private static string GetErrorDetails(Exception error)
        {
            return error == null ? null : error.ToString();
        }
    }
}
