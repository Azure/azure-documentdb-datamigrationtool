using Microsoft.DataTransfer.RavenDb.Shared;
using Microsoft.DataTransfer.WpfHost.Basics.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.RavenDb.Wpf.Shared
{
    partial class RavenDbDataAdapterConnectionConfigurationControl : UserControl
    {
        public static readonly DependencyProperty ConfigurationProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<RavenDbDataAdapterConnectionConfigurationControl>(c => c.Configuration),
            typeof(IRavenDbDataAdapterConfiguration),
            typeof(RavenDbDataAdapterConnectionConfigurationControl),
            new FrameworkPropertyMetadata(ConfigurationChanged));

        private RavenDbDataAdapterConnectionConfigurationViewModel ViewModel
        {
            get { return (RavenDbDataAdapterConnectionConfigurationViewModel)LayoutRoot.DataContext; }
            set { LayoutRoot.DataContext = value; }
        }

        public IRavenDbDataAdapterConfiguration Configuration
        {
            get { return (IRavenDbDataAdapterConfiguration)GetValue(ConfigurationProperty); }
            set { SetValue(ConfigurationProperty, value); }
        }

        public RavenDbDataAdapterConnectionConfigurationControl()
        {
            InitializeComponent();

            ViewModel = new RavenDbDataAdapterConnectionConfigurationViewModel();
        }

        private static void ConfigurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as RavenDbDataAdapterConnectionConfigurationControl;
            if (self == null)
                return;

            self.ViewModel.Configuration = e.NewValue as IRavenDbDataAdapterConfiguration;
        }
    }
}
