using Microsoft.DataTransfer.WpfHost.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Steps;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.WpfHost.Steps.Welcome
{
    sealed class WelcomeStep : NavigationStepBase, IInformationalStep
    {
        public override string Title { get { return StepsResources.WelcomeStepTitle; } }

        public WelcomeStep(IDataTransferModel transferModel)
            : base(transferModel)
        {
            transferModel.Subscribe(m => m.HasImportStarted, OnImportStateChanged);
        }

        private void OnImportStateChanged(bool hasImportStarted)
        {
            IsAllowed = !hasImportStarted;
        }

        protected override UserControl CreatePresenter()
        {
            return new WelcomePage();
        }
    }
}
