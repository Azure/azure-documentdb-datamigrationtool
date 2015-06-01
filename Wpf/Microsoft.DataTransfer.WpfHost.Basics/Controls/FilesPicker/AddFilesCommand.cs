using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Basics.Controls.EditableItemsList;
using Microsoft.Win32;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker
{
    sealed class AddFilesCommand : EditItemsCollectionCommandBase<string>
    {
        private IFileDialogConfiguration configuration;

        public AddFilesCommand(IFileDialogConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

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
                    Items.Add(filename);
        }
    }
}
