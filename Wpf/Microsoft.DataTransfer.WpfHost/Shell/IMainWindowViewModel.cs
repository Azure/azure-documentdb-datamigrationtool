using Microsoft.DataTransfer.WpfHost.ServiceModel;
using Microsoft.DataTransfer.WpfHost.Shell.Navigation;

namespace Microsoft.DataTransfer.WpfHost.Shell
{
    interface IMainWindowViewModel
    {
        INavigationService NavigationService { get; }
        NavigationActionsViewModel NavigationActions { get; }
    }
}
