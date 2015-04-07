using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Basics;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using Microsoft.DataTransfer.WpfHost.Shell.Navigation;

namespace Microsoft.DataTransfer.WpfHost.Shell
{
    sealed class MainWindowViewModel : BindableBase, IMainWindowViewModel
    {
        public INavigationService NavigationService { get; private set; }

        public NavigationActionsViewModel NavigationActions { get; private set; }

        public MainWindowViewModel(IApplicationController applicationController, INavigationService navigationService, IDataTransferModel transferModel)
        {
            Guard.NotNull("navigationService", navigationService);

            NavigationService = navigationService;
            NavigationActions = new NavigationActionsViewModel(applicationController, navigationService, transferModel);
        }
    }
}
