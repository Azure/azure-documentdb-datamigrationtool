
namespace Microsoft.DataTransfer.WpfHost.ServiceModel
{
    /// <summary>
    /// Allows basic manipulations with task bar icon.
    /// </summary>
    public interface ITaskBarService
    {
        /// <summary>
        /// Flash the task bar icon to inform the user that the window requires attention.
        /// </summary>
        /// <returns>true if window was not active and notification was given; otherwise, false.</returns>
        bool Notify();
    }
}
