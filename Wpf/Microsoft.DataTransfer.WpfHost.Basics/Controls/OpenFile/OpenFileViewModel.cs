using System.Windows.Input;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.OpenFile
{
    sealed class OpenFileViewModel: BindableBase, IFileNameListener
    {
        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { SetProperty(ref fileName, value); }
        }

        public ICommand SelectFile { get; private set; }

        public OpenFileViewModel(IFileDialogConfiguration configuration)
        {
            SelectFile = new OpenFileCommand(this, configuration);
        }
    }
}
