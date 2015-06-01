using Microsoft.DataTransfer.Basics;
using System.Linq;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.EditableItemsList
{
    sealed class EditItemCommand<T> : EditItemsCollectionCommandBase<T>
    {
        private readonly ISingleItemProvider<T> itemProvider;
        private readonly ISelectedItemsProvider selectedItemsProvider;

        public EditItemCommand(ISingleItemProvider<T> itemProvider, ISelectedItemsProvider selectedItemsProvider)
        {
            Guard.NotNull("itemProvider", itemProvider);
            Guard.NotNull("selectedItemsProvider", selectedItemsProvider);

            this.itemProvider = itemProvider;
            this.selectedItemsProvider = selectedItemsProvider;
        }

        public override void Execute(object parameter)
        {
            var selectedItems = selectedItemsProvider.GetIndices().ToArray();

            if (!selectedItems.Any())
                return;

            var selectedItemIndex = selectedItems[0];

            itemProvider.SetItem(Items[selectedItemIndex]);
            Items.RemoveAt(selectedItemIndex);
        }
    }
}
