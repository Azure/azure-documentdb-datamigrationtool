using Microsoft.DataTransfer.WpfHost.Basics;
using System.Windows.Input;

namespace Microsoft.DataTransfer.WpfHost.Extensibility.Basics.Controls.SaveFile
{
    sealed class SaveFileViewModel : BindableBase, IFileNameListener
    {
        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { SetProperty(ref fileName, value); }
        }

        public ICommand SelectFile { get; private set; }

        public SaveFileViewModel(IFileDialogConfiguration configuration)
        {
            SelectFile = new SaveFileCommand(this, configuration);
        }
    }
}
