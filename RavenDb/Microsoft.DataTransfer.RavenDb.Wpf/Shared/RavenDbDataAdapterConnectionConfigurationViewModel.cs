using System.Windows.Input;
using Microsoft.DataTransfer.RavenDb.Shared;
using Microsoft.DataTransfer.WpfHost.Basics;

namespace Microsoft.DataTransfer.RavenDb.Wpf.Shared
{
    sealed class RavenDbDataAdapterConnectionConfigurationViewModel : BindableBase
    {
        private IRavenDbDataAdapterConfiguration configuration;

        public IRavenDbDataAdapterConfiguration Configuration
        {
            get { return configuration; }
            set { SetProperty(ref configuration, value); }
        }

        public ICommand TestConnection { get; private set; }

        public RavenDbDataAdapterConnectionConfigurationViewModel()
        {
            TestConnection = new TestConnectionCommand();
        }
    }
}
