using System.Windows.Input;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.SaveFile
{
    sealed class SaveLocalFileViewModel : BindableBase, IFileNameListener
    {
        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { SetProperty(ref fileName, value); }
        }

        public ICommand SelectFile { get; private set; }

        public SaveLocalFileViewModel(IFileDialogConfiguration configuration)
        {
            SelectFile = new SaveFileCommand(this, configuration);
        }
    }
}
