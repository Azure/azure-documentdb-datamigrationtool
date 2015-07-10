using Microsoft.DataTransfer.WpfHost.Basics.Controls.EditableItemsList;
using Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker.Urls.Http;
using System.Windows;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker
{
    sealed class AddUrlCommand : EditItemsCollectionCommandBase<string>
    {
        public override void Execute(object parameter)
        {
            var dialog = new AddHttpUrlsDialog
            {
                Owner = Application.Current.MainWindow
            };

            if (dialog.ShowDialog() != true)
                return;

            foreach (var url in dialog.Urls)
                Items.Add(url);
        }
    }
}
