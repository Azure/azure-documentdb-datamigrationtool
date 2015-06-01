using System.Collections.Generic;
using System.Windows.Input;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.EditableItemsList
{
    sealed class EditableItemsListViewModel<T> : BindableBase
    {
        private EditItemsCollectionCommandBase<T> addItem;
        private EditItemsCollectionCommandBase<T> editItem;
        private EditItemsCollectionCommandBase<T> removeItems;

        private IList<T> items;

        public IList<T> Items
        {
            get { return items; }
            set
            {
                SetProperty(ref items, value);
                addItem.Items = editItem.Items = removeItems.Items = items;
            }
        }

        public ICommand AddItem
        {
            get { return addItem; }
        }

        public ICommand EditItem
        {
            get { return editItem; }
        }

        public ICommand RemoveItems
        {
            get { return removeItems; }
        }

        public EditableItemsListViewModel(ISingleItemProvider<T> newItemProvider, ISelectedItemsProvider selectedItemsProvider)
        {
            addItem = new AddItemCommand<T>(newItemProvider);
            editItem = new EditItemCommand<T>(newItemProvider, selectedItemsProvider);
            removeItems = new RemoveItemsCommand<T>(selectedItemsProvider);
        }
    }
}
