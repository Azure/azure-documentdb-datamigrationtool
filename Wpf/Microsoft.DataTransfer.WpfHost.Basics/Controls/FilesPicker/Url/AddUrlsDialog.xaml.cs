using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker.Url
{
    partial class AddUrlsDialog : Window, IUrlsListener
    {
        public IEnumerable<string> Urls { get; private set; }

        private AddUrlsViewModel ViewModel
        {
            get { return (AddUrlsViewModel)DataContext; }
            set { DataContext = value; }
        }

        public AddUrlsDialog()
        {
            InitializeComponent();
            ViewModel = new AddUrlsViewModel(this);
        }

        public void Notify(IEnumerable<string> urls)
        {
            Urls = urls;
            DialogResult = true;
            Close();
        }

        protected override void OnActivated(EventArgs e)
        {
            if (String.IsNullOrEmpty(ViewModel.Urls))
                SniffUrlsFromClipboard();
            base.OnActivated(e);
        }

        private void SniffUrlsFromClipboard()
        {
            ViewModel.Urls = String.Join(
                Environment.NewLine, 
                Clipboard
                    .GetText(TextDataFormat.UnicodeText)
                    .Replace("\t", String.Empty)
                    .Split(new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(l => UrlHelper.IsValidUrl(l)));
        }
    }
}
