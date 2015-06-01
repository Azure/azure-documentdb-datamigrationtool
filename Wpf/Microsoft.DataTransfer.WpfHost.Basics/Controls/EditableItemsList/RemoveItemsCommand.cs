using Microsoft.DataTransfer.Basics;
using System.Linq;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.EditableItemsList
{
    sealed class RemoveItemsCommand<T> : EditItemsCollectionCommandBase<T>
    {
        private readonly ISelectedItemsProvider itemsProvider;

        public RemoveItemsCommand(ISelectedItemsProvider itemsProvider)
        {
            Guard.NotNull("itemsProvider", itemsProvider);

            this.itemsProvider = itemsProvider;
        }

        public override void Execute(object parameter)
        {
            var indices = itemsProvider.GetIndices();
            if (indices == null)
                return;

            var removedCount = 0;
            foreach (var index in indices.OrderBy(i => i))
                Items.RemoveAt(index - (removedCount++));

            itemsProvider.ClearSelection();
        }
    }
}
