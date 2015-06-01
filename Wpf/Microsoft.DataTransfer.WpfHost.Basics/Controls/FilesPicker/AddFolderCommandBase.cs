using Microsoft.DataTransfer.WpfHost.Basics.Controls.EditableItemsList;
using System.Windows.Forms;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker
{
    abstract class AddFolderCommandBase : EditItemsCollectionCommandBase<string>
    {
        public sealed override void Execute(object parameter)
        {
            using (var dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = false,
                Description = Resources.FolderBrowserDescription
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    Items.Add(GetFolderSearchPattern(dialog.SelectedPath));
            }
        }

        protected abstract string GetFolderSearchPattern(string folderPath);
    }
}
