using System.Collections;
using System.Linq;

namespace Microsoft.DataTransfer.WpfHost.Extensibility.Basics.Controls.FilesPicker
{
    sealed class RemoveFilesCommand : EditFilesCollectionCommandBase
    {
        public override void Execute(object parameter)
        {
            var selectedItems = parameter as IEnumerable;
            if (selectedItems == null)
                return;

            foreach (var file in selectedItems.OfType<string>().ToArray())
                Files.Remove(file);
        }
    }
}
