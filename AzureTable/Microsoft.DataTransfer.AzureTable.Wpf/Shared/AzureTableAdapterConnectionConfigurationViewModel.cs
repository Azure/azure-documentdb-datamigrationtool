using Microsoft.DataTransfer.AzureTable.Shared;
using Microsoft.DataTransfer.WpfHost.Basics;
using System.Windows.Input;

namespace Microsoft.DataTransfer.AzureTable.Wpf.Shared
{
    sealed class AzureTableAdapterConnectionConfigurationViewModel : BindableBase
    {
        private IAzureTableAdapterConfiguration configuration;

        public IAzureTableAdapterConfiguration Configuration
        {
            get { return configuration; }
            set { SetProperty(ref configuration, value); }
        }

        public ICommand TestConnection { get; private set; }

        public AzureTableAdapterConnectionConfigurationViewModel()
        {
            TestConnection = new TestConnectionCommand();
        }
    }
}
