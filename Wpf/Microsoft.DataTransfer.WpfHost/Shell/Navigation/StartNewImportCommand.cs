using Microsoft.DataTransfer.WpfHost.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Steps;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Microsoft.DataTransfer.WpfHost.Shell.Navigation
{
    sealed class StartNewImportCommand : NavigateStepCommandBase
    {
        private IDataTransferModel transferModel;
        private IApplicationController applicationController;

        public StartNewImportCommand(IApplicationController applicationController, INavigationService navigationService, IDataTransferModel transferModel)
            : base(navigationService)
        {
            this.transferModel = transferModel;
            this.applicationController = applicationController;

            transferModel.Subscribe(m => m.ImportCancellation, OnCancellationChanged);
        }

        public override bool CanExecute(object parameter)
        {
            return transferModel.ImportCancellation == null && base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            switch (MessageBox.Show(
                Application.Current.MainWindow,
                Resources.NewImportDialogText, Resources.NewImportDialogTitle,
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
            {
                case MessageBoxResult.Yes:
                    var newNavigationService = applicationController.ResetState();
                    newNavigationService.CurrentStep = GetTargetStep(newNavigationService);
                    break;
                case MessageBoxResult.No:
                    transferModel.HasImportStarted = false;
                    base.Execute(parameter);
                    break;
            }
        }

        protected override INavigationStep GetTargetStep()
        {
            return GetTargetStep(NavigationService);
        }

        private static INavigationStep GetTargetStep(INavigationService navigationService)
        {
            return navigationService.Steps.FirstOrDefault(s => !(s is IInformationalStep));
        }

        private void OnCancellationChanged(CancellationTokenSource cancellation)
        {
            RaiseCanExecuteChanged();
        }
    }
}
