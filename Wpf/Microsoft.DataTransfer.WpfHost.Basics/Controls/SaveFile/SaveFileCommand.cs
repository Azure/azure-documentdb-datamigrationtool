using Microsoft.DataTransfer.WpfHost.Basics.Commands;
using Microsoft.Win32;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.SaveFile
{
    sealed class SaveFileCommand : CommandBase
    {
        private IFileNameListener listener;
        private IFileDialogConfiguration configuration;

        public SaveFileCommand(IFileNameListener listener, IFileDialogConfiguration configuration)
        {
            this.listener = listener;
            this.configuration = configuration;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var dialog = new SaveFileDialog
            {
                AddExtension = true,
                OverwritePrompt = true,
                ValidateNames = true,
                Filter = configuration.Filter,
                DefaultExt = configuration.DefaultExtension
            };

            if (dialog.ShowDialog() == true)
                listener.FileName = dialog.FileName;
        }
    }
}
