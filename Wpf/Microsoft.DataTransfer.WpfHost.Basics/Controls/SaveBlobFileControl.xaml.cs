using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.Basics.Controls.SaveFile;
using System;
using System.ComponentModel;
using System.Windows;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls
{
    /// <summary>
    /// Provides a way to create a new blob or overwrite an existing one.
    /// </summary>
    public partial class SaveBlobFileControl : DataBoundUserControl
    {
        /// <summary>
        /// Identifies the <see cref="SaveBlobFileControl.BlobUrl" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty BlobUrlProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<SaveBlobFileControl>(c => c.BlobUrl),
            typeof(string), typeof(SaveBlobFileControl),
            new FrameworkPropertyMetadata(String.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        
        private SaveBlobFileViewModel ViewModel
        {
            get { return GetViewModel<SaveBlobFileViewModel>(); }
        }

        /// <summary>
        /// Gets the selected BLOB URL.
        /// </summary>
        public string BlobUrl
        {
            get { return (string)GetValue(BlobUrlProperty); }
            set { SetValue(BlobUrlProperty, value); }
        }

        /// <summary>
        /// Creates a new instance of <see cref="SaveBlobFileControl" />.
        /// </summary>
        public SaveBlobFileControl()
            : base(new SaveBlobFileViewModel())
        {
            InitializeComponent();
            LayoutRoot.DataContext = ViewModel;
        }

        /// <summary>
        /// Handles property changes of the view model.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="PropertyChangedEventArgs" /> that contains the event data.</param>
        protected override void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == SaveBlobFileViewModel.BlobUrlPropertyName)
            {
                SetValueSuppressHandler(BlobUrlProperty, ViewModel.BlobUrl);
            }
        }

        /// <summary>
        /// Handles dependency property changes.
        /// </summary>
        /// <param name="e">A <see cref="DependencyPropertyChangedEventArgs" /> that contains the event data.</param>
        protected override void OnDependencyPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == BlobUrlProperty)
            {
                ViewModel.BlobUrl = (string)e.NewValue;
            }
        }
    }
}
