using Microsoft.Win32;

namespace Microsoft.DataTransfer.WpfHost.Extensibility.Basics.Controls.FilesPicker
{
    sealed class AddFilesCommand : EditFilesCollectionCommandBase
    {
        private IFileDialogConfiguration configuration;

        public AddFilesCommand(IFileDialogConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public override void Execute(object parameter)
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = configuration.Filter,
                DefaultExt = configuration.DefaultExtension
            };

            if (dialog.ShowDialog() == true)
                foreach (var filename in dialog.FileNames)
                    Files.Add(filename);
        }
    }
}
