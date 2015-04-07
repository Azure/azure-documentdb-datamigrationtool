using Microsoft.DataTransfer.WpfHost.Basics;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Steps;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DataTransfer.WpfHost.Model
{
    sealed class NavigationService : BindableBase, INavigationService
    {
        private IEnumerable<INavigationStep> steps;
        private INavigationStep currentStep;

        public IEnumerable<INavigationStep> Steps
        {
            get { return steps; }
            private set { SetProperty(ref steps, value); }
        }

        public INavigationStep CurrentStep
        {
            get { return currentStep; }
            set { SetProperty(ref currentStep, value); }
        }

        public NavigationService(IEnumerable<INavigationStep> steps)
        {
            Steps = steps;

            CurrentStep = steps.FirstOrDefault(s => s.IsAllowed);
            if (CurrentStep == null)
                throw Errors.NoAvailableSteps();
        }
    }
}
