using Microsoft.DataTransfer.WpfHost.ServiceModel.Steps;
using System.Collections.Generic;
using System.ComponentModel;

namespace Microsoft.DataTransfer.WpfHost.ServiceModel
{
    /// <summary>
    /// Provides step-based navigation functionality.
    /// </summary>
    public interface INavigationService : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the collection of all known steps.
        /// </summary>
        IEnumerable<INavigationStep> Steps { get; }

        /// <summary>
        /// Gets or sets the current step.
        /// </summary>
        INavigationStep CurrentStep { get; set; }
    }
}
