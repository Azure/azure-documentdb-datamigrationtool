using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker.Urls.Http
{
    partial class AddHttpUrlsDialog : Window, IValueListener<IEnumerable<string>>
    {
        public IEnumerable<string> Urls { get; private set; }

        private AddHttpUrlsViewModel ViewModel
        {
            get { return (AddHttpUrlsViewModel)DataContext; }
            set { DataContext = value; }
        }

        public AddHttpUrlsDialog()
        {
            InitializeComponent();
            ViewModel = new AddHttpUrlsViewModel(this);
        }

        public void Notify(IEnumerable<string> urls)
        {
            Urls = urls;
            DialogResult = true;
        }

        protected override void OnActivated(EventArgs e)
        {
            if (String.IsNullOrEmpty(ViewModel.Value))
                SniffUrlsFromClipboard();
            base.OnActivated(e);
        }

        private void SniffUrlsFromClipboard()
        {
            ViewModel.Value = String.Join(
                Environment.NewLine, 
                Clipboard
                    .GetText(TextDataFormat.UnicodeText)
                    .Replace("\t", String.Empty)
                    .Split(new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(l => UrlHelper.IsValidUrl(l)));

            txtUrls.CaretIndex = int.MaxValue;
        }
    }
}
