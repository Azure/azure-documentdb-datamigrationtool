using Microsoft.DataTransfer.ServiceModel;
using Microsoft.DataTransfer.WpfHost.Extensibility;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.WpfHost.Steps.SourceSetup
{
    sealed class SourceSetupStep : AdapterSetupStepBase
    {
        public override string Title
        {
            get { return StepsResources.SourceSetupStepTitle; }
        }

        public SourceSetupStep(IDataTransferService transferService, IDataAdapterConfigurationProvidersCollection configurationProviders,
            IDataTransferModel transferModel)
            : base(transferService, configurationProviders, transferModel)
        {
            Initialize();
        }

        protected override UserControl CreatePresenter()
        {
            var sourceAdapters = TransferService.GetKnownSources();

            if (sourceAdapters.Any())
                TransferModel.SourceAdapterName = sourceAdapters.Keys.First();

            return new SourceSetupPage()
            {
                DataContext = new SourceSetupViewModel(sourceAdapters, ConfigurationProviders.GetForSource, TransferModel)
            };
        }

        protected override bool IsTransferModelValid()
        {
            return TransferModel.SourceAdapterName != null && TransferModel.SourceConfiguration != null;
        }
    }
}
