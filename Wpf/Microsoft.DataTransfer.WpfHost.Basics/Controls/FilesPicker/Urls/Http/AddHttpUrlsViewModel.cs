using System.Collections.Generic;
using System.Windows.Input;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker.Urls.Http
{
    sealed class AddHttpUrlsViewModel : BindableBase, IValueProvider<string>
    {
        private string urls;
        private ICommand setUrls;

        public string Value
        {
            get { return urls; }
            set { SetProperty(ref urls, value); }
        }

        public ICommand SetUrls
        {
            get { return setUrls; }
        }

        public AddHttpUrlsViewModel(IValueListener<IEnumerable<string>> listener)
        {
            setUrls = new SetHttpUrlsCommand(this, listener);
        }
    }
}
