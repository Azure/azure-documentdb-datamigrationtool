using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Basics.Commands;
using Microsoft.Win32;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.OpenFile
{
    sealed class OpenFileCommand : CommandBase
    {
        private IFileNameListener listener;
        private IFileDialogConfiguration configuration;

        public OpenFileCommand(IFileNameListener listener, IFileDialogConfiguration configuration)
        {
            Guard.NotNull("listener", listener);
            Guard.NotNull("configuration", configuration);

            this.listener = listener;
            this.configuration = configuration;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var dialog = new OpenFileDialog
            {
                Filter = configuration.Filter,
                DefaultExt = configuration.DefaultExtension
            };

            if (dialog.ShowDialog() == true)
                listener.FileName = dialog.FileName;
        }
    }
}
