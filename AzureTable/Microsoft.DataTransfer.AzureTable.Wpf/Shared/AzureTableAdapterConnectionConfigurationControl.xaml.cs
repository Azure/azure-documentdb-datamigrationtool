using Microsoft.DataTransfer.AzureTable.Shared;
using Microsoft.DataTransfer.Basics.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.AzureTable.Wpf.Shared
{
    partial class AzureTableAdapterConnectionConfigurationControl : UserControl
    {
        public static readonly DependencyProperty ConfigurationProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<AzureTableAdapterConnectionConfigurationControl>(c => c.Configuration),
            typeof(IAzureTableAdapterConfiguration),
            typeof(AzureTableAdapterConnectionConfigurationControl),
            new FrameworkPropertyMetadata(ConfigurationChanged));

        private AzureTableAdapterConnectionConfigurationViewModel ViewModel
        {
            get { return (AzureTableAdapterConnectionConfigurationViewModel)LayoutRoot.DataContext; }
            set { LayoutRoot.DataContext = value; }
        }

        public IAzureTableAdapterConfiguration Configuration
        {
            get { return (IAzureTableAdapterConfiguration)GetValue(ConfigurationProperty); }
            set { SetValue(ConfigurationProperty, value); }
        }

        public AzureTableAdapterConnectionConfigurationControl()
        {
            InitializeComponent();

            ViewModel = new AzureTableAdapterConnectionConfigurationViewModel();
        }

        private static void ConfigurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as AzureTableAdapterConnectionConfigurationControl;
            if (self == null)
                return;

            self.ViewModel.Configuration = e.NewValue as IAzureTableAdapterConfiguration;
        }
    }
}
