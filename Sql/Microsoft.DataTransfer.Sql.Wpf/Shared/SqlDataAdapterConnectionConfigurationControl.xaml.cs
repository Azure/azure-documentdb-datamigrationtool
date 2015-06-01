using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.Sql.Shared;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.Sql.Wpf.Shared
{
    partial class SqlDataAdapterConnectionConfigurationControl : UserControl
    {
        public static readonly DependencyProperty ConfigurationProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<SqlDataAdapterConnectionConfigurationControl>(c => c.Configuration),
            typeof(ISqlDataAdapterConfiguration),
            typeof(SqlDataAdapterConnectionConfigurationControl),
            new FrameworkPropertyMetadata(ConfigurationChanged));

        private SqlDataAdapterConnectionConfigurationViewModel ViewModel
        {
            get { return (SqlDataAdapterConnectionConfigurationViewModel)LayoutRoot.DataContext; }
            set { LayoutRoot.DataContext = value; }
        }

        public ISqlDataAdapterConfiguration Configuration
        {
            get { return (ISqlDataAdapterConfiguration)GetValue(ConfigurationProperty); }
            set { SetValue(ConfigurationProperty, value); }
        }

        public SqlDataAdapterConnectionConfigurationControl()
        {
            InitializeComponent();

            ViewModel = new SqlDataAdapterConnectionConfigurationViewModel();
        }

        private static void ConfigurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as SqlDataAdapterConnectionConfigurationControl;
            if (self == null)
                return;

            self.ViewModel.Configuration = e.NewValue as ISqlDataAdapterConfiguration;
        }
    }
}
