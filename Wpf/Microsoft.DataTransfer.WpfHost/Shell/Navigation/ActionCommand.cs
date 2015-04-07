using Microsoft.DataTransfer.WpfHost.ServiceModel;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Steps;
using System.Linq;

namespace Microsoft.DataTransfer.WpfHost.Shell.Navigation
{
    sealed class ActionCommand : NavigateStepCommandBase
    {
        public ActionCommand(INavigationService navigationService)
            : base(navigationService) { }

        protected override INavigationStep GetTargetStep()
        {
            return NavigationService.Steps.FirstOrDefault(s => s is IActionStep);
        }

        public override void Execute(object parameter)
        {
            base.Execute(parameter);

            var actionStep = NavigationService.CurrentStep as IActionStep;
            if (actionStep == null)
                return;

            actionStep.Execute();
        }
    }
}
