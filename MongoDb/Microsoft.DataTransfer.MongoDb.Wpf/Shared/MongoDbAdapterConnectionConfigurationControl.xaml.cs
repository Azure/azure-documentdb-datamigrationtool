using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.MongoDb.Shared;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.MongoDb.Wpf.Shared
{
    partial class MongoDbAdapterConnectionConfigurationControl : UserControl
    {
        public static readonly DependencyProperty ConfigurationProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<MongoDbAdapterConnectionConfigurationControl>(c => c.Configuration),
            typeof(IMongoDbAdapterConfiguration),
            typeof(MongoDbAdapterConnectionConfigurationControl),
            new FrameworkPropertyMetadata(ConfigurationChanged));

        private MongoDbAdapterConnectionConfigurationViewModel ViewModel
        {
            get { return (MongoDbAdapterConnectionConfigurationViewModel)LayoutRoot.DataContext; }
            set { LayoutRoot.DataContext = value; }
        }

        public IMongoDbAdapterConfiguration Configuration
        {
            get { return (IMongoDbAdapterConfiguration)GetValue(ConfigurationProperty); }
            set { SetValue(ConfigurationProperty, value); }
        }

        public MongoDbAdapterConnectionConfigurationControl()
        {
            InitializeComponent();

            ViewModel = new MongoDbAdapterConnectionConfigurationViewModel();
        }

        private static void ConfigurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as MongoDbAdapterConnectionConfigurationControl;
            if (self == null)
                return;

            self.ViewModel.Configuration = e.NewValue as IMongoDbAdapterConfiguration;
        }
    }
}
