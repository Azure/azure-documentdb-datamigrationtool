
namespace Microsoft.DataTransfer.WpfHost.Extensibility.Basics.Controls.EditableItemsList
{
    interface ISingleItemProvider<T>
    {
        T GetItem();
        void ClearItem();
    }
}
