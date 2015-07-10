using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.HBase.Shared;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.HBase.Wpf.Shared
{
    partial class HBaseAdapterConnectionConfigurationControl : UserControl
    {
        public static readonly DependencyProperty ConfigurationProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<HBaseAdapterConnectionConfigurationControl>(c => c.Configuration),
            typeof(IHBaseAdapterConfiguration),
            typeof(HBaseAdapterConnectionConfigurationControl),
            new FrameworkPropertyMetadata(ConfigurationChanged));

        private HBaseAdapterConnectionConfigurationViewModel ViewModel
        {
            get { return (HBaseAdapterConnectionConfigurationViewModel)LayoutRoot.DataContext; }
            set { LayoutRoot.DataContext = value; }
        }

        public IHBaseAdapterConfiguration Configuration
        {
            get { return (IHBaseAdapterConfiguration)GetValue(ConfigurationProperty); }
            set { SetValue(ConfigurationProperty, value); }
        }

        public HBaseAdapterConnectionConfigurationControl()
        {
            InitializeComponent();

            ViewModel = new HBaseAdapterConnectionConfigurationViewModel();
        }

        private static void ConfigurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as HBaseAdapterConnectionConfigurationControl;
            if (self == null)
                return;

            self.ViewModel.Configuration = e.NewValue as IHBaseAdapterConfiguration;
        }
    }
}
