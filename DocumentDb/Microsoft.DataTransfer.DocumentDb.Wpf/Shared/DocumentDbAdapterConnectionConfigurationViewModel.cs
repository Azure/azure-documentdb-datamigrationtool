using Microsoft.DataTransfer.DocumentDb.Shared;
using Microsoft.DataTransfer.WpfHost.Basics;
using Microsoft.DataTransfer.WpfHost.Basics.Extensions;
using System.Windows.Input;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Shared
{
    sealed class DocumentDbAdapterConnectionConfigurationViewModel : BindableBase
    {
        private IDocumentDbAdapterConfiguration configuration;

        public IDocumentDbAdapterConfiguration Configuration
        {
            get { return configuration; }
            set { SetProperty(ref configuration, value); }
        }

        public ICommand TestConnection { get; private set; }

        public DocumentDbAdapterConnectionConfigurationViewModel()
        {
            TestConnection = new TestConnectionCommand();
        }
    }
}
