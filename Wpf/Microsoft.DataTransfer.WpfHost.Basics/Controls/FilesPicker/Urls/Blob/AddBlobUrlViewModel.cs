using Microsoft.DataTransfer.WpfHost.Basics.Controls.Shared;
using System.Windows.Input;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker.Urls.Blob
{
    sealed class AddBlobUrlViewModel : BlobUrlViewModelBase, IValueProvider<string>
    {
        private ICommand setUrl;

        private string blobUrl;

        public ICommand SetUrl
        {
            get { return setUrl; }
        }

        public string Value
        {
            get { return blobUrl; }
            private set { SetProperty(ref blobUrl, value); }
        }

        public AddBlobUrlViewModel(IValueListener<string> listener)
        {
            setUrl = new SetBlobUrlCommand(this, listener);
        }

        protected override void SetBlobUrl(string url)
        {
            Value = url;
        }
    }
}
