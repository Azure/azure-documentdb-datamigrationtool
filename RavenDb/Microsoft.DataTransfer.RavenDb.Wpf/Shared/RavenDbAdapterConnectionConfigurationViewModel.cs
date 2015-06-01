using Microsoft.DataTransfer.RavenDb.Shared;
using Microsoft.DataTransfer.WpfHost.Basics;
using System.Windows.Input;

namespace Microsoft.DataTransfer.RavenDb.Wpf.Shared
{
    sealed class RavenDbAdapterConnectionConfigurationViewModel : BindableBase
    {
        private IRavenDbAdapterConfiguration configuration;

        public IRavenDbAdapterConfiguration Configuration
        {
            get { return configuration; }
            set { SetProperty(ref configuration, value); }
        }

        public ICommand TestConnection { get; private set; }

        public RavenDbAdapterConnectionConfigurationViewModel()
        {
            TestConnection = new TestConnectionCommand();
        }
    }
}
