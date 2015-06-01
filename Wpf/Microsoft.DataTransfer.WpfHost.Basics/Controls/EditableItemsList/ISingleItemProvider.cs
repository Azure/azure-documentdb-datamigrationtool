
namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.EditableItemsList
{
    interface ISingleItemProvider<T>
    {
        T GetItem();
        void SetItem(T item);
    }
}
