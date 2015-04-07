using Microsoft.DataTransfer.WpfHost.ServiceModel;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Steps;

namespace Microsoft.DataTransfer.WpfHost.Shell.Navigation
{
    sealed class NavigateToPreviousStepCommand : NavigateStepCommandBase
    {
        public NavigateToPreviousStepCommand(INavigationService navigationService)
            : base(navigationService) { }

        protected override INavigationStep GetTargetStep()
        {
            var currentStep = NavigationService.CurrentStep;

            INavigationStep previousAllowedStep = null;
            foreach (var step in NavigationService.Steps)
            {
                if (step == currentStep)
                    break;

                if (step.IsAllowed)
                    previousAllowedStep = step;
            }

            return previousAllowedStep;
        }
    }
}
