using Microsoft.DataTransfer.MongoDb.Shared;
using Microsoft.DataTransfer.WpfHost.Basics;
using System.Windows.Input;

namespace Microsoft.DataTransfer.MongoDb.Wpf.Shared
{
    sealed class MongoDbAdapterConnectionConfigurationViewModel : BindableBase
    {
        private IMongoDbAdapterConfiguration configuration;

        public IMongoDbAdapterConfiguration Configuration
        {
            get { return configuration; }
            set { SetProperty(ref configuration, value); }
        }

        public ICommand TestConnection { get; private set; }

        public MongoDbAdapterConnectionConfigurationViewModel()
        {
            TestConnection = new TestConnectionCommand();
        }
    }
}
