using System;
using System.Linq;
using System.Windows;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker.Urls.Blob
{
    partial class AddBlobUrlDialog : Window, IValueListener<string>
    {
        public string Url { get; private set; }

        private AddBlobUrlViewModel ViewModel
        {
            get { return (AddBlobUrlViewModel)DataContext; }
            set { DataContext = value; }
        }

        public AddBlobUrlDialog()
        {
            InitializeComponent();
            ViewModel = new AddBlobUrlViewModel(this);
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            MinHeight = ActualHeight;
            MaxHeight = ActualHeight;
        }

        public void Notify(string url)
        {
            Url = url;
            DialogResult = true;
        }
    }
}
