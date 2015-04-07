using Microsoft.DataTransfer.WpfHost.Basics.Commands;
using Microsoft.DataTransfer.WpfHost.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Steps;

namespace Microsoft.DataTransfer.WpfHost.Shell.Navigation
{
    abstract class NavigateStepCommandBase : CommandBase
    {
        protected INavigationService NavigationService { get; private set; }

        public NavigateStepCommandBase(INavigationService navigationService)
        {
            NavigationService = navigationService;

            navigationService.Subscribe(s => s.CurrentStep, RaiseCanExecuteChanged);
            foreach (var step in navigationService.Steps)
                step.Subscribe(s => s.IsAllowed, RaiseCanExecuteChanged);
        }

        public override bool CanExecute(object parameter)
        {
            return GetTargetStep() != null;
        }

        public override void Execute(object parameter)
        {
            var targetStep = GetTargetStep();
            if (targetStep == null)
            {
                RaiseCanExecuteChanged();
                return;
            }

            NavigationService.CurrentStep = targetStep;
        }

        private void RaiseCanExecuteChanged<T>(T nothing)
        {
            RaiseCanExecuteChanged();
        }

        protected abstract INavigationStep GetTargetStep();
    }
}
