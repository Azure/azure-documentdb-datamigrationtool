using Microsoft.DataTransfer.WpfHost.Basics;
using System.Collections.Generic;
using System.Windows.Input;

namespace Microsoft.DataTransfer.WpfHost.Extensibility.Basics.Controls.FilesPicker
{
    sealed class FilesPickerViewModel : BindableBase
    {
        private EditFilesCollectionCommandBase addFiles;
        private EditFilesCollectionCommandBase addSingleFolder;
        private EditFilesCollectionCommandBase addRecursiveFolder;
        private EditFilesCollectionCommandBase removeFiles;

        private ICollection<string> files;

        public ICollection<string> Files
        {
            get { return files; }
            set
            {
                SetProperty(ref files, value);
                addFiles.Files = addSingleFolder.Files =
                    addRecursiveFolder.Files = removeFiles.Files = files;
            }
        }

        public ICommand AddFiles
        {
            get { return addFiles; }
        }

        public ICommand AddSingleFolder
        {
            get { return addSingleFolder; }
        }

        public ICommand AddRecursiveFolder
        {
            get { return addRecursiveFolder; }
        }

        public ICommand RemoveFiles
        {
            get { return removeFiles; }
        }

        public FilesPickerViewModel(IFileDialogConfiguration configuration)
        {
            addFiles = new AddFilesCommand(configuration);
            addSingleFolder = new AddSingleFolderCommand();
            addRecursiveFolder = new AddRecursiveFolderCommand();
            removeFiles = new RemoveFilesCommand();
        }
    }
}
