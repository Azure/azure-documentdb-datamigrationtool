using Microsoft.DataTransfer.Basics.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls
{
    /// <summary>
    /// Contains a list of selectable items. This extended version allows to get indices of all selected items.
    /// </summary>
    public class ExtendedListBox : ListBox
    {
        private static readonly DependencyPropertyKey SelectedIndicesPropertyKey = DependencyProperty.RegisterReadOnly(
            ObjectExtensions.MemberName<ExtendedListBox>(c => c.SelectedIndices),
            typeof(IEnumerable<int>), typeof(ExtendedListBox),
            new FrameworkPropertyMetadata(Enumerable.Empty<int>()));

        /// <summary>
        /// Identifies the <see cref="ExtendedListBox.SelectedIndices" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedIndicesProperty = SelectedIndicesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the indices of all currently selected items.
        /// </summary>
        public IEnumerable<int> SelectedIndices
        {
            get { return (IEnumerable<int>)GetValue(SelectedIndicesProperty); }
            private set { SetValue(SelectedIndicesPropertyKey, value); }
        }

        /// <summary>
        /// Updates list of selected indices.
        /// </summary>
        /// <param name="e">Provides data for <see cref="SelectionChangedEventArgs" />.</param>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            SelectedIndices = GetSelectedIndices();
        }

        private IEnumerable<int> GetSelectedIndices()
        {
            var result = new List<int>(SelectedItems.Count);

            var count = ItemContainerGenerator.Items.Count;
            for (var index = 0; index < count; ++index)
            {
                var container = ItemContainerGenerator.ContainerFromIndex(index) as ListBoxItem;
                if (container != null && container.IsSelected)
                    result.Add(index);
            }

            return result;
        }
    }
}
