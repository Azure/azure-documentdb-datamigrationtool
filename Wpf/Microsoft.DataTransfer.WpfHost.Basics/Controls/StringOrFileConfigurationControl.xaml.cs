using Microsoft.DataTransfer.Basics.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls
{
    /// <summary>
    /// Provides a way to define a raw string value or select a file instead.
    /// </summary>
    public partial class StringOrFileConfigurationControl : UserControl
    {
        /// <summary>
        /// Identifies the <see cref="StringOrFileConfigurationControl.UseFile" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty UseFileProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<StringOrFileConfigurationControl>(c => c.UseFile),
            typeof(bool), typeof(StringOrFileConfigurationControl),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Identifies the <see cref="StringOrFileConfigurationControl.StringValueLabel" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty StringValueLabelProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<StringOrFileConfigurationControl>(c => c.StringValueLabel), typeof(string), typeof(StringOrFileConfigurationControl));

        /// <summary>
        /// Identifies the <see cref="StringOrFileConfigurationControl.StringValue" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty StringValueProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<StringOrFileConfigurationControl>(c => c.StringValue),
            typeof(string), typeof(StringOrFileConfigurationControl),
            new FrameworkPropertyMetadata(String.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Identifies the <see cref="StringOrFileConfigurationControl.StringEditorContextMenu" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty StringEditorContextMenuProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<StringOrFileConfigurationControl>(c => c.StringEditorContextMenu),
            typeof(ContextMenu), typeof(StringOrFileConfigurationControl));

        /// <summary>
        /// Identifies the <see cref="StringOrFileConfigurationControl.FileNameLabel" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileNameLabelProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<StringOrFileConfigurationControl>(c => c.FileNameLabel), typeof(string), typeof(StringOrFileConfigurationControl));

        /// <summary>
        /// Identifies the <see cref="StringOrFileConfigurationControl.FileName" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<StringOrFileConfigurationControl>(c => c.FileName),
            typeof(string), typeof(StringOrFileConfigurationControl),
            new FrameworkPropertyMetadata(String.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Identifies the <see cref="StringOrFileConfigurationControl.FileFilter" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileFilterProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<StringOrFileConfigurationControl>(c => c.FileFilter), typeof(string), typeof(StringOrFileConfigurationControl));

        /// <summary>
        /// Identifies the <see cref="StringOrFileConfigurationControl.FileDefaultExtension" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileDefaultExtensionProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<StringOrFileConfigurationControl>(c => c.FileDefaultExtension), typeof(string), typeof(StringOrFileConfigurationControl));

        /// <summary>
        /// Gets or sets the value indicating whether file option is selected.
        /// </summary>
        public bool UseFile
        {
            get { return (bool)GetValue(UseFileProperty); }
            set { SetValue(UseFileProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label text for raw string value selector.
        /// </summary>
        public string StringValueLabel
        {
            get { return (string)GetValue(StringValueLabelProperty); }
            set { SetValue(StringValueLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the string value.
        /// </summary>
        public string StringValue
        {
            get { return (string)GetValue(StringValueProperty); }
            set { SetValue(StringValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the string editor context menu.
        /// </summary>
        public ContextMenu StringEditorContextMenu
        {
            get { return (ContextMenu)GetValue(StringEditorContextMenuProperty); }
            set { SetValue(StringEditorContextMenuProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label text for file name selector.
        /// </summary>
        public string FileNameLabel
        {
            get { return (string)GetValue(FileNameLabelProperty); }
            set { SetValue(FileNameLabelProperty, value); }
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
        public string FileFilter
        {
            get { return (string)GetValue(FileFilterProperty); }
            set { SetValue(FileFilterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the default file extension for the file picker dialog.
        /// </summary>
        public string FileDefaultExtension
        {
            get { return (string)GetValue(FileDefaultExtensionProperty); }
            set { SetValue(FileDefaultExtensionProperty, value); }
        }

        /// <summary>
        /// Creates a new instance of <see cref="StringOrFileConfigurationControl" />.
        /// </summary>
        public StringOrFileConfigurationControl()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }
    }
}
