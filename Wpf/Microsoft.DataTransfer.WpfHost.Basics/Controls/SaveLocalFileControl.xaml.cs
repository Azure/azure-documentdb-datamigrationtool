using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.Basics.Controls.SaveFile;
using Microsoft.DataTransfer.WpfHost.Basics.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls
{
    /// <summary>
    /// Provides a way to create a new file or overwrite an existing one.
    /// </summary>
    public partial class SaveLocalFileControl : UserControl, IFileDialogConfiguration
    {
        /// <summary>
        /// Identifies the <see cref="SaveLocalFileControl.FileName" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<SaveFileControl>(c => c.FileName),
            typeof(string), typeof(SaveLocalFileControl),
            new FrameworkPropertyMetadata(String.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Identifies the <see cref="SaveLocalFileControl.Filter" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<SaveLocalFileControl>(c => c.Filter), typeof(string), typeof(SaveLocalFileControl));

        /// <summary>
        /// Identifies the <see cref="SaveLocalFileControl.DefaultExtension" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultExtensionProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<SaveLocalFileControl>(c => c.DefaultExtension), typeof(string), typeof(SaveLocalFileControl));

        private SaveLocalFileViewModel ViewModel
        {
            get { return (SaveLocalFileViewModel)LayoutRoot.DataContext; }
            set { LayoutRoot.DataContext = value; }
        }

        /// <summary>
        /// Gets or sets the selected file name.
        /// </summary>
        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the extensions filter string for the file picker dialog.
        /// </summary>
        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the default file extension for the file picker dialog.
        /// </summary>
        public string DefaultExtension
        {
            get { return (string)GetValue(DefaultExtensionProperty); }
            set { SetValue(DefaultExtensionProperty, value); }
        }

        /// <summary>
        /// Creates a new instance of <see cref="SaveLocalFileControl" />.
        /// </summary>
        public SaveLocalFileControl()
        {
            InitializeComponent();
            ViewModel = new SaveLocalFileViewModel(this);
            ViewModel.Subscribe(m => m.FileName, SetFileName);
        }

        private void SetFileName(string fileName)
        {
            FileName = fileName;
        }
    }
}
