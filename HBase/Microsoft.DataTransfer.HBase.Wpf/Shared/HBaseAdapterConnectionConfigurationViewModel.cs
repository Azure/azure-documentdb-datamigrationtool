using Microsoft.DataTransfer.HBase.Shared;
using Microsoft.DataTransfer.WpfHost.Basics;
using System.Windows.Input;

namespace Microsoft.DataTransfer.HBase.Wpf.Shared
{
    sealed class HBaseAdapterConnectionConfigurationViewModel : BindableBase
    {
        private IHBaseAdapterConfiguration configuration;

        public IHBaseAdapterConfiguration Configuration
        {
            get { return configuration; }
            set { SetProperty(ref configuration, value); }
        }

        public ICommand TestConnection { get; private set; }

        public HBaseAdapterConnectionConfigurationViewModel()
        {
            TestConnection = new TestConnectionCommand();
        }
    }
}
