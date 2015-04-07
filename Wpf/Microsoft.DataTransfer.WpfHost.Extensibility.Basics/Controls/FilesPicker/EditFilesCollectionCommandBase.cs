using Microsoft.DataTransfer.WpfHost.Basics.Commands;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.WpfHost.Extensibility.Basics.Controls.FilesPicker
{
    abstract class EditFilesCollectionCommandBase : CommandBase
    {
        private ICollection<string> files;

        public ICollection<string> Files
        {
            get { return files; }
            set
            {
                files = value;
                RaiseCanExecuteChanged();
            }
        }

        public override bool CanExecute(object parameter)
        {
            return files != null;
        }
    }
}
