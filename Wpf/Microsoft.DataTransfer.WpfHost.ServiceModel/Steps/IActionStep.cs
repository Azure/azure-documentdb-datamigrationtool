using System.Threading.Tasks;

namespace Microsoft.DataTransfer.WpfHost.ServiceModel.Steps
{
    /// <summary>
    /// Represents navigation step that is capable of executing long-running action.
    /// </summary>
    public interface IActionStep : INavigationStep
    {
        /// <summary>
        /// Performs the long-running step action.
        /// </summary>
        /// <returns>Task that represent asynchronous operation.</returns>
        Task Execute(); 
    }
}
