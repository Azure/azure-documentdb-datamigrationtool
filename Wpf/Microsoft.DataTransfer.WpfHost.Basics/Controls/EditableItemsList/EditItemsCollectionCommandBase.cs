using Microsoft.DataTransfer.WpfHost.Basics.Commands;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.EditableItemsList
{
    abstract class EditItemsCollectionCommandBase<T> : CommandBase
    {
        private IList<T> items;

        public IList<T> Items
        {
            get { return items; }
            set
            {
                items = value;
                RaiseCanExecuteChanged();
            }
        }

        public override bool CanExecute(object parameter)
        {
            return items != null;
        }
    }
}
