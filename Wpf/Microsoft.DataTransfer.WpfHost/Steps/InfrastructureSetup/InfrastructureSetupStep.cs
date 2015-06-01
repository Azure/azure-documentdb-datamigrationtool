using Microsoft.DataTransfer.WpfHost.Helpers;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.WpfHost.Steps.InfrastructureSetup
{
    sealed class InfrastructureSetupStep : NavigationStepBase
    {
        private InfrastructureConfiguration infrastructureConfiguration;

        public override string Title
        {
	        get { return StepsResources.InfrastructureStepTitle; }
        }

        public InfrastructureSetupStep(IDataTransferModel transferModel)
            : base(transferModel)
        {
            transferModel.InfrastructureConfiguration = infrastructureConfiguration = new InfrastructureConfiguration();
            transferModel.PropertyChanged += OnTransferModelPropertyChanged;
            OnTransferModelPropertyChanged(this, new PropertyChangedEventArgs(DataTransferModelProperties.HasImportStarted));
        }

        protected override UserControl CreatePresenter()
        {
            return new InfrastructureSetupPage()
            {
                DataContext = infrastructureConfiguration
            };
        }

        private void OnTransferModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsValid = IsTransferModelValid();

            if (e.PropertyName == DataTransferModelProperties.HasImportStarted)
                IsAllowed = !TransferModel.HasImportStarted;
        }

        private bool IsTransferModelValid()
        {
            return true;
        }
    }
}
