using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.Basics.Net;
using Microsoft.DataTransfer.WpfHost.Basics.Controls.SaveFile;
using System;
using System.ComponentModel;
using System.Windows;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls
{
    /// <summary>
    /// Provides a way to create a new file or overwrite existing one.
    /// </summary>
    public partial class SaveFileControl : DataBoundUserControl
    {
        /// <summary>
        /// Identifies the <see cref="SaveFileControl.FileName" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<SaveFileControl>(c => c.FileName),
            typeof(string), typeof(SaveFileControl),
            new FrameworkPropertyMetadata(String.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        
        /// <summary>
        /// Identifies the <see cref="SaveFileControl.LocalFileFilter" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty LocalFileFilterProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<SaveFileControl>(c => c.LocalFileFilter),
            typeof(string), typeof(SaveFileControl));

        /// <summary>
        /// Identifies the <see cref="SaveFileControl.LocalFileDefaultExtension" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty LocalFileDefaultExtensionProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<SaveFileControl>(c => c.LocalFileDefaultExtension),
            typeof(string), typeof(SaveFileControl));
        
        /// <summary>
        /// Identifies the <see cref="SaveFileControl.Overwrite" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty OverwriteProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<SaveFileControl>(c => c.Overwrite),
            typeof(bool), typeof(SaveFileControl),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private SaveFileViewModel ViewModel
        {
            get { return GetViewModel<SaveFileViewModel>(); }
        }

        /// <summary>
        /// Gets the selected file name.
        /// </summary>
        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the extensions filter string for the file picker dialog.
        /// </summary>
        public string LocalFileFilter
        {
            get { return (string)GetValue(LocalFileFilterProperty); }
            set { SetValue(LocalFileFilterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the default file extension for the file picker dialog.
        /// </summary>
        public string LocalFileDefaultExtension
        {
            get { return (string)GetValue(LocalFileDefaultExtensionProperty); }
            set { SetValue(LocalFileDefaultExtensionProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the target file should be overwritten.
        /// </summary>
        public bool Overwrite
        {
            get { return (bool)GetValue(OverwriteProperty); }
            set { SetValue(OverwriteProperty, value); }
        }

        /// <summary>
        /// Creates a new instance of <see cref="SaveFileControl" />.
        /// </summary>
        public SaveFileControl()
            : base(new SaveFileViewModel())
        {
            InitializeComponent();
            LayoutRoot.DataContext = ViewModel;
            Loaded += SyncProperties;
        }

        private void SyncProperties(object sender, RoutedEventArgs e)
        {
            // Sync Overwrite property between view model and dependency property, as it has a hardcoded value
            SetValueSuppressHandler(OverwriteProperty, ViewModel.Overwrite);
        }

        /// <summary>
        /// Handles property changes of the view model.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="PropertyChangedEventArgs" /> that contains the event data.</param>
        protected override void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == SaveFileViewModel.UseBlobPropertyName)
            {
                if (ViewModel.UseBlob)
                {
                    SetValueSuppressHandler(FileNameProperty, ViewModel.BlobUrl);
                }
                else
                {
                    SetValueSuppressHandler(FileNameProperty, ViewModel.LocalFile);
                }
            }
            else if (e.PropertyName == SaveFileViewModel.LocalFilePropertyName && !ViewModel.UseBlob)
            {
                SetValueSuppressHandler(FileNameProperty, ViewModel.LocalFile);
            }
            else if (e.PropertyName == SaveFileViewModel.BlobUrlPropertyName && ViewModel.UseBlob)
            {
                SetValueSuppressHandler(FileNameProperty, ViewModel.BlobUrl);
            }
            else if (e.PropertyName == SaveFileViewModel.OverwritePropertyName)
            {
                SetValueSuppressHandler(OverwriteProperty, ViewModel.Overwrite);
            }
        }

        /// <summary>
        /// Handles changes of the dependency properties.
        /// </summary>
        /// <param name="e">A <see cref="DependencyPropertyChangedEventArgs" /> that contains the event data.</param>
        protected override void OnDependencyPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == FileNameProperty)
            {
                var fileName = (string)e.NewValue;

                ViewModel.UseBlob = BlobUri.IsValid(fileName);
                if (ViewModel.UseBlob)
                {
                    ViewModel.BlobUrl = fileName;
                }
                else
                {
                    ViewModel.LocalFile = fileName;
                }
            }
            else if (e.Property == LocalFileFilterProperty)
            {
                ViewModel.LocalFileFilter = (string)e.NewValue;
            }
            else if (e.Property == LocalFileDefaultExtensionProperty)
            {
                ViewModel.LocalFileDefaultExtension = (string)e.NewValue;
            }
            else if (e.Property == OverwriteProperty)
            {
                ViewModel.Overwrite = (bool)e.NewValue;
            }
        }
    }
}
