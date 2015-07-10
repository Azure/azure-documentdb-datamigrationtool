using Microsoft.DataTransfer.DynamoDb.Shared;
using Microsoft.DataTransfer.WpfHost.Basics;
using System.Windows.Input;

namespace Microsoft.DataTransfer.DynamoDb.Wpf.Shared
{
    sealed class DynamoDbAdapterConnectionConfigurationViewModel : BindableBase
    {
        private IDynamoDbAdapterConfiguration configuration;

        public IDynamoDbAdapterConfiguration Configuration
        {
            get { return configuration; }
            set { SetProperty(ref configuration, value); }
        }

        public ICommand TestConnection { get; private set; }

        public DynamoDbAdapterConnectionConfigurationViewModel()
        {
            TestConnection = new TestConnectionCommand();
        }
    }
}
