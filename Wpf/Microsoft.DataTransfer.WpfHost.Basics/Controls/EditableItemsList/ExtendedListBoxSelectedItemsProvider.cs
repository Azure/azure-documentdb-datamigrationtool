using Microsoft.DataTransfer.Basics;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.EditableItemsList
{
    sealed class ExtendedListBoxSelectedItemsProvider : ISelectedItemsProvider
    {
        private readonly ExtendedListBox source;

        public ExtendedListBoxSelectedItemsProvider(ExtendedListBox source)
        {
            Guard.NotNull("source", source);
            this.source = source;
        }

        public IEnumerable<int> GetIndices()
        {
            return source.SelectedIndices;
        }

        public void ClearSelection()
        {
            source.SelectedItem = null;
        }
    }
}
