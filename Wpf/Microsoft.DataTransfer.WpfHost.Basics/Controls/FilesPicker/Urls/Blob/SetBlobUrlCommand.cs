using System;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker.Urls.Blob
{
    sealed class SetBlobUrlCommand : ValueMediator<string, string>
    {
        public SetBlobUrlCommand(IValueProvider<string> provider, IValueListener<string> listener)
            : base(provider, listener) { }

        public override bool CanExecute(object parameter)
        {
            var url = parameter as string ?? ReadValue();
            return !String.IsNullOrEmpty(url) && !url.Equals(Resources.BlobUrlSample, StringComparison.Ordinal)
                && UrlHelper.IsValidBlobUrl(url);
        }

        public override void Execute(object parameter)
        {
            var url = ReadValue();

            if (!CanExecute(url))
                return;

            WriteValue(url);
        }
    }
}
