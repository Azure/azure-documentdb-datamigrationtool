using Microsoft.DataTransfer.WpfHost.Basics;
using Microsoft.DataTransfer.WpfHost.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Steps;
using System.Windows.Input;

namespace Microsoft.DataTransfer.WpfHost.Shell.Navigation
{
    sealed class NavigationActionsViewModel : BindableBase
    {
        private bool showPreviousStepButton;
        private bool showNextStepButton;
        private bool showImportButton;
        private bool showNewImportButton;
        private bool showCancelImportButton;

        public ICommand NavigateToPreviousStep { get; private set; }
        public ICommand NavigateToNextStep { get; private set; }
        public ICommand StartImport { get; private set; }
        public ICommand StartNewImport { get; private set; }
        public ICommand CancelImport { get; private set; }

        public bool ShowNavigateToPreviousStepButton
        {
            get { return showPreviousStepButton; }
            private set { SetProperty(ref showPreviousStepButton, value); }
        }

        public bool ShowNavigateToNextStepButton
        {
            get { return showNextStepButton; }
            private set { SetProperty(ref showNextStepButton, value); }
        }

        public bool ShowStartImportButton
        {
            get { return showImportButton; }
            private set { SetProperty(ref showImportButton, value); }
        }

        public bool ShowStartNewImportButton
        {
            get { return showNewImportButton; }
            private set { SetProperty(ref showNewImportButton, value); }
        }

        public bool ShowCancelImportButton
        {
            get { return showCancelImportButton; }
            private set { SetProperty(ref showCancelImportButton, value); }
        }

        public NavigationActionsViewModel(IApplicationController applicationController, INavigationService navigationService, IDataTransferModel transferModel)
        {
            NavigateToPreviousStep = new NavigateToPreviousStepCommand(navigationService);
            NavigateToNextStep = new NavigateToNextStepCommand(navigationService);
            StartImport = new ActionCommand(navigationService);
            StartNewImport = new StartNewImportCommand(applicationController, navigationService, transferModel);
            CancelImport = new CancelImportCommand(transferModel);

            navigationService.Subscribe(s => s.CurrentStep, OnCurrentStepChanged);
        }

        private void OnCurrentStepChanged(INavigationStep newStep)
        {
            var isSummaryStep = newStep is ISummaryStep;
            var isImportStep = newStep is IActionStep;

            ShowNavigateToPreviousStepButton = !isImportStep;
            ShowNavigateToNextStepButton = !isImportStep && !isSummaryStep;
            ShowStartImportButton = isSummaryStep;
            ShowStartNewImportButton = ShowCancelImportButton = isImportStep;
        }
    }
}
