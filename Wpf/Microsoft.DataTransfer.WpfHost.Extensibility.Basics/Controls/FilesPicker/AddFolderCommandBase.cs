using System.Windows.Forms;

namespace Microsoft.DataTransfer.WpfHost.Extensibility.Basics.Controls.FilesPicker
{
    abstract class AddFolderCommandBase : EditFilesCollectionCommandBase
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
                    Files.Add(GetFolderSearchPattern(dialog.SelectedPath));
            }
        }

        protected abstract string GetFolderSearchPattern(string folderPath);
    }
}
