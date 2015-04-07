using System.Windows;

namespace Microsoft.DataTransfer.WpfHost.ServiceModel
{
    /// <summary>
    /// Provides root application resources.
    /// </summary>
    public interface IApplicationController
    {
        /// <summary>
        /// Provides application main window.
        /// </summary>
        /// <returns>Main window.</returns>
        Window GetMainWindow();

        /// <summary>
        /// Resets application state.
        /// </summary>
        /// <returns><see cref="INavigationService" /> to be used with new application state.</returns>
        INavigationService ResetState();
    }
}
