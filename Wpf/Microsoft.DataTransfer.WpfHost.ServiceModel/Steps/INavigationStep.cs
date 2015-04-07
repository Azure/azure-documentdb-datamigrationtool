using System.ComponentModel;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.WpfHost.ServiceModel.Steps
{
    /// <summary>
    /// Represents basic navigation step.
    /// </summary>
    public interface INavigationStep : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the step title.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the value indicating whether step content is valid or not.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Gets the value indicating whether navigation can be performed to the step.
        /// </summary>
        bool IsAllowed { get; }

        /// <summary>
        /// Gets the step presenter control.
        /// </summary>
        UserControl Presenter { get; }
    }
}
