using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.Basics.Controls.EditableItemsList;
using Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls
{
    /// <summary>
    /// Provides a way to select one or more files.
    /// </summary>
    public partial class FilesPickerControl : UserControl, IFileDialogConfiguration
    {
        /// <summary>
        /// Identifies the <see cref="FilesPickerControl.Files" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty FilesProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<FilesPickerControl>(c => c.Files),
            typeof(IList<string>), typeof(FilesPickerControl),
            new FrameworkPropertyMetadata(OnFilesPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="FilesPickerControl.Filter" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<FilesPickerControl>(c => c.Filter), typeof(string), typeof(FilesPickerControl));

        /// <summary>
        /// Identifies the <see cref="FilesPickerControl.DefaultExtension" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultExtensionProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<FilesPickerControl>(c => c.DefaultExtension), typeof(string), typeof(FilesPickerControl));

        private FilesPickerViewModel ViewModel
        {
            get { return (FilesPickerViewModel)LayoutRoot.DataContext; }
            set { LayoutRoot.DataContext = value; }
        }

        /// <summary>
        /// Gets or sets the collection of selected files.
        /// </summary>
        public IList<string> Files
        {
            get { return (IList<string>)GetValue(FilesProperty); }
            set { SetValue(FilesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the extensions filter string for the files picker dialog.
        /// </summary>
        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the default file extension for the files picker dialog.
        /// </summary>
        public string DefaultExtension
        {
            get { return (string)GetValue(DefaultExtensionProperty); }
            set { SetValue(DefaultExtensionProperty, value); }
        }

        /// <summary>
        /// Creates a new instance of <see cref="FilesPickerControl" />.
        /// </summary>
        public FilesPickerControl()
        {
            InitializeComponent();
            ViewModel = new FilesPickerViewModel(this, new ExtendedListBoxSelectedItemsProvider(lbFiles));
        }

        private static void OnFilesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as FilesPickerControl;
            if (self == null)
                return;

            self.ViewModel.Files = self.Files;
        }
    }
}
