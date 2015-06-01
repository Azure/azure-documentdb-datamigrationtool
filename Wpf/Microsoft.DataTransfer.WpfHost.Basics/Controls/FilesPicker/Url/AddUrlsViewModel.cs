using System.Windows.Input;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker.Url
{
    sealed class AddUrlsViewModel : BindableBase, IUrlsProvider
    {
        private string urls;
        private ICommand setUrls;

        public string Urls
        {
            get { return urls; }
            set { SetProperty(ref urls, value); }
        }

        public ICommand SetUrls
        {
            get { return setUrls; }
        }

        public AddUrlsViewModel(IUrlsListener listener)
        {
            setUrls = new SetUrlsCommand(this, listener);
        }
    }
}
