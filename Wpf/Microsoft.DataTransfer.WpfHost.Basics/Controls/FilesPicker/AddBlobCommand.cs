using Microsoft.DataTransfer.WpfHost.Basics.Controls.EditableItemsList;
using Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker.Urls.Blob;
using System.Windows;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker
{
    sealed class AddBlobCommand : EditItemsCollectionCommandBase<string>
    {
        public override void Execute(object parameter)
        {
            var dialog = new AddBlobUrlDialog
            {
                Owner = Application.Current.MainWindow
            };

            if (dialog.ShowDialog() != true)
                return;

            Items.Add(dialog.Url);
        }
    }
}
