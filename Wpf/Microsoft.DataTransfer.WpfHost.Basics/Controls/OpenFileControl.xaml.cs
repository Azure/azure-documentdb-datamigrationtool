using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.Basics.Controls.OpenFile;
using Microsoft.DataTransfer.WpfHost.Basics.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls
{
    /// <summary>
    /// Provides a way to select single file.
    /// </summary>
    public partial class OpenFileControl : UserControl, IFileDialogConfiguration
    {
        /// <summary>
        /// Identifies the <see cref="OpenFileControl.FileName" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<SaveFileControl>(c => c.FileName),
            typeof(string), typeof(OpenFileControl),
            new FrameworkPropertyMetadata(String.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Identifies the <see cref="OpenFileControl.Filter" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<OpenFileControl>(c => c.Filter), typeof(string), typeof(OpenFileControl));

        /// <summary>
        /// Identifies the <see cref="OpenFileControl.DefaultExtension" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultExtensionProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<OpenFileControl>(c => c.DefaultExtension), typeof(string), typeof(OpenFileControl));

        private OpenFileViewModel ViewModel
        {
            get { return (OpenFileViewModel)LayoutRoot.DataContext; }
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
        /// Creates a new instance of <see cref="OpenFileControl" />.
        /// </summary>
        public OpenFileControl()
        {
            InitializeComponent();
            ViewModel = new OpenFileViewModel(this);
            ViewModel.Subscribe(m => m.FileName, SetFileName);
        }

        private void SetFileName(string fileName)
        {
            FileName = fileName;
        }
    }
}
