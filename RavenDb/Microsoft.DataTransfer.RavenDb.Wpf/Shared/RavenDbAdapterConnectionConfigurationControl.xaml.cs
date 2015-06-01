using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.RavenDb.Shared;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.RavenDb.Wpf.Shared
{
    partial class RavenDbAdapterConnectionConfigurationControl : UserControl
    {
        public static readonly DependencyProperty ConfigurationProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<RavenDbAdapterConnectionConfigurationControl>(c => c.Configuration),
            typeof(IRavenDbAdapterConfiguration),
            typeof(RavenDbAdapterConnectionConfigurationControl),
            new FrameworkPropertyMetadata(ConfigurationChanged));

        private RavenDbAdapterConnectionConfigurationViewModel ViewModel
        {
            get { return (RavenDbAdapterConnectionConfigurationViewModel)LayoutRoot.DataContext; }
            set { LayoutRoot.DataContext = value; }
        }

        public IRavenDbAdapterConfiguration Configuration
        {
            get { return (IRavenDbAdapterConfiguration)GetValue(ConfigurationProperty); }
            set { SetValue(ConfigurationProperty, value); }
        }

        public RavenDbAdapterConnectionConfigurationControl()
        {
            InitializeComponent();

            ViewModel = new RavenDbAdapterConnectionConfigurationViewModel();
        }

        private static void ConfigurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as RavenDbAdapterConnectionConfigurationControl;
            if (self == null)
                return;

            self.ViewModel.Configuration = e.NewValue as IRavenDbAdapterConfiguration;
        }
    }
}
