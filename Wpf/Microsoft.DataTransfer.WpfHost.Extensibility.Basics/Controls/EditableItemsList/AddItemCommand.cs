using Microsoft.DataTransfer.Basics;

namespace Microsoft.DataTransfer.WpfHost.Extensibility.Basics.Controls.EditableItemsList
{
    sealed class AddItemCommand<T> : EditItemsCollectionCommandBase<T>
    {
        private readonly ISingleItemProvider<T> itemProvider;

        public AddItemCommand(ISingleItemProvider<T> itemProvider)
        {
            Guard.NotNull("newItemProvider", itemProvider);

            this.itemProvider = itemProvider;
        }

        public override void Execute(object parameter)
        {
            var item = itemProvider.GetItem();
            if (item == null)
                return;

            Items.Add(item);
            itemProvider.ClearItem();
        }
    }
}
