using Microsoft.DataTransfer.ServiceModel;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using System.Linq;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.WpfHost.Steps.SinkSetup
{
    sealed class SinkSetupStep : AdapterSetupStepBase
    {
        public override string Title
        {
            get { return StepsResources.SinkSetupStepTitle; }
        }

        public SinkSetupStep(IDataTransferService transferService, IDataAdapterConfigurationProvidersCollection configurationProviders,
            IDataTransferModel transferModel)
            : base(transferService, configurationProviders, transferModel)
        {
            Initialize();
        }

        protected override UserControl CreatePresenter()
        {
            var sinkAdapters = TransferService.GetKnownSinks();

            if (sinkAdapters.Any())
                TransferModel.SinkAdapterName = sinkAdapters.Keys.First();

            return new SinkSetupPage()
            {
                DataContext = new SinkSetupViewModel(sinkAdapters, ConfigurationProviders.GetForSink, TransferModel)
            };
        }

        protected override bool IsTransferModelValid()
        {
            return TransferModel.SinkAdapterName != null && TransferModel.SinkConfiguration != null;
        }
    }
}
