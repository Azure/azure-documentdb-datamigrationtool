using Microsoft.DataTransfer.WpfHost.Basics;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Configuration;
using System.Threading;

namespace Microsoft.DataTransfer.WpfHost.Model
{
    sealed class DataTransferModel : BindableBase, IDataTransferModel
    {
        private IInfrastructureConfiguration infrastructureConfiguration;

        private string sourceAdapterName;
        private object sourceConfiguration;

        private string sinkAdapterName;
        private object sinkConfiguration;

        private bool hasImportStarted;
        private CancellationTokenSource importCancellation;

        public IInfrastructureConfiguration InfrastructureConfiguration
        {
            get { return infrastructureConfiguration; }
            set { SetProperty(ref infrastructureConfiguration, value); }
        }

        public string SourceAdapterName
        {
            get { return sourceAdapterName; }
            set { SetProperty(ref sourceAdapterName, value); }
        }

        public object SourceConfiguration
        {
            get { return sourceConfiguration; }
            set { SetProperty(ref sourceConfiguration, value); }
        }

        public string SinkAdapterName
        {
            get { return sinkAdapterName; }
            set { SetProperty(ref sinkAdapterName, value); }
        }

        public object SinkConfiguration
        {
            get { return sinkConfiguration; }
            set { SetProperty(ref sinkConfiguration, value); }
        }

        public bool HasImportStarted
        {
            get { return hasImportStarted; }
            set { SetProperty(ref hasImportStarted, value); }
        }

        public CancellationTokenSource ImportCancellation
        {
            get { return importCancellation; }
            set { SetProperty(ref importCancellation, value); }
        }
    }
}
