using Microsoft.DataTransfer.WpfHost.ServiceModel;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Steps;

namespace Microsoft.DataTransfer.WpfHost.Shell.Navigation
{
    sealed class NavigateToNextStepCommand : NavigateStepCommandBase
    {
        public NavigateToNextStepCommand(INavigationService navigationService)
            : base(navigationService) { }

        protected override INavigationStep GetTargetStep()
        {
            var currentStep = NavigationService.CurrentStep;

            var foundCurrent = false;
            foreach (var step in NavigationService.Steps)
            {
                if (foundCurrent)
                {
                    if (step.IsAllowed)
                        return step;
                }
                else
                {
                    if (step == currentStep)
                        foundCurrent = true;
                }
            }

            return null;
        }
    }
}
