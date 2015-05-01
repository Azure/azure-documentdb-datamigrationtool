using System.Collections.Generic;

namespace Microsoft.DataTransfer.WpfHost.Extensibility.Basics.Controls.EditableItemsList
{
    interface ISelectedItemsProvider
    {
        IEnumerable<int> GetIndices();
        void ClearSelection();
    }
}
