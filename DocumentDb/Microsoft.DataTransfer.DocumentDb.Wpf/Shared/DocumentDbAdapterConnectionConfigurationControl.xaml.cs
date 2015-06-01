using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.DocumentDb.Shared;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Shared
{
    partial class DocumentDbAdapterConnectionConfigurationControl : UserControl
    {
        public static readonly DependencyProperty ConfigurationProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<DocumentDbAdapterConnectionConfigurationControl>(c => c.Configuration),
            typeof(IDocumentDbAdapterConfiguration),
            typeof(DocumentDbAdapterConnectionConfigurationControl),
            new FrameworkPropertyMetadata(ConfigurationChanged));

        private DocumentDbAdapterConnectionConfigurationViewModel ViewModel
        {
            get { return (DocumentDbAdapterConnectionConfigurationViewModel)LayoutRoot.DataContext; }
            set { LayoutRoot.DataContext = value; }
        }

        public IDocumentDbAdapterConfiguration Configuration
        {
            get { return (IDocumentDbAdapterConfiguration)GetValue(ConfigurationProperty); }
            set { SetValue(ConfigurationProperty, value); }
        }

        public DocumentDbAdapterConnectionConfigurationControl()
        {
            InitializeComponent();

            ViewModel = new DocumentDbAdapterConnectionConfigurationViewModel();
        }

        private static void ConfigurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as DocumentDbAdapterConnectionConfigurationControl;
            if (self == null)
                return;

            self.ViewModel.Configuration = e.NewValue as IDocumentDbAdapterConfiguration;
        }
    }
}
