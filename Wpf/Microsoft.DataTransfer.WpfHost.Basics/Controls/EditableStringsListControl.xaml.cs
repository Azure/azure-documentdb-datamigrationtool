using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.Basics.Controls.EditableItemsList;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls
{
    /// <summary>
    /// Provides a way to specify one or more string items.
    /// </summary>
    public partial class EditableStringsListControl : UserControl
    {
        /// <summary>
        /// Identifies the <see cref="EditableStringsListControl.Items" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<EditableStringsListControl>(c => c.Items),
            typeof(IList<string>), typeof(EditableStringsListControl),
            new FrameworkPropertyMetadata(OnItemsPropertyChanged));

        private EditableItemsListViewModel<string> ViewModel
        {
            get { return (EditableItemsListViewModel<string>)LayoutRoot.DataContext; }
            set { LayoutRoot.DataContext = value; }
        }

        /// <summary>
        /// Gets or sets the collection of items.
        /// </summary>
        public IList<string> Items
        {
            get { return (IList<string>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        /// <summary>
        /// Creates a new instance of <see cref="EditableStringsListControl" />.
        /// </summary>
        public EditableStringsListControl()
        {
            InitializeComponent();
            ViewModel = new EditableItemsListViewModel<string>(
                new TextBoxStringItemProvider(txtNewItem), new ExtendedListBoxSelectedItemsProvider(lbItems));
        }

        private void HandleItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            HandleEditItem(e);
        }

        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                HandleEditItem(e);
        }

        private void HandleEditItem(RoutedEventArgs e)
        {
            if (ViewModel.EditItem.CanExecute(null))
                ViewModel.EditItem.Execute(null);

            e.Handled = true;
        }

        private static void OnItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as EditableStringsListControl;
            if (self == null)
                return;

            self.ViewModel.Items = self.Items;
        }

        sealed class TextBoxStringItemProvider : ISingleItemProvider<string>
        {
            private readonly TextBox source;

            public TextBoxStringItemProvider(TextBox source)
            {
                Guard.NotNull("source", source);
                this.source = source;
            }

            public string GetItem()
            {
                var newItem = source.Text;
                return String.IsNullOrEmpty(newItem) ? null : newItem;
            }

            public void SetItem(string item)
            {
                source.Text = item;
                source.CaretIndex = Int32.MaxValue;
                source.Focus();
            }
        }
    }
}
