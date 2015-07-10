using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.DynamoDb.Shared;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.DynamoDb.Wpf.Shared
{
    partial class DynamoDbAdapterConnectionConfigurationControl : UserControl
    {
        public static readonly DependencyProperty ConfigurationProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<DynamoDbAdapterConnectionConfigurationControl>(c => c.Configuration),
            typeof(IDynamoDbAdapterConfiguration),
            typeof(DynamoDbAdapterConnectionConfigurationControl),
            new FrameworkPropertyMetadata(ConfigurationChanged));

        private DynamoDbAdapterConnectionConfigurationViewModel ViewModel
        {
            get { return (DynamoDbAdapterConnectionConfigurationViewModel)LayoutRoot.DataContext; }
            set { LayoutRoot.DataContext = value; }
        }

        public IDynamoDbAdapterConfiguration Configuration
        {
            get { return (IDynamoDbAdapterConfiguration)GetValue(ConfigurationProperty); }
            set { SetValue(ConfigurationProperty, value); }
        }

        public DynamoDbAdapterConnectionConfigurationControl()
        {
            InitializeComponent();

            ViewModel = new DynamoDbAdapterConnectionConfigurationViewModel();
        }

        private static void ConfigurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as DynamoDbAdapterConnectionConfigurationControl;
            if (self == null)
                return;

            self.ViewModel.Configuration = e.NewValue as IDynamoDbAdapterConfiguration;
        }
    }
}
