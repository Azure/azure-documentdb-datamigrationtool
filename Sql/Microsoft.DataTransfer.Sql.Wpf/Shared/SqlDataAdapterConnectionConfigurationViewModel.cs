using Microsoft.DataTransfer.Sql.Shared;
using Microsoft.DataTransfer.WpfHost.Basics;
using System.Windows.Input;

namespace Microsoft.DataTransfer.Sql.Wpf.Shared
{
    sealed class SqlDataAdapterConnectionConfigurationViewModel : BindableBase
    {
        private ISqlDataAdapterConfiguration configuration;

        public ISqlDataAdapterConfiguration Configuration
        {
            get { return configuration; }
            set { SetProperty(ref configuration, value); }
        }

        public ICommand TestConnection { get; private set; }

        public SqlDataAdapterConnectionConfigurationViewModel()
        {
            TestConnection = new TestConnectionCommand();
        }
    }
}
