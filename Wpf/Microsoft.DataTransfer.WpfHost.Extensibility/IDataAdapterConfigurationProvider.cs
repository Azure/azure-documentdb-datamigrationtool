using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.WpfHost.Extensibility
{
    /// <summary>
    /// Provides configuration for data adapter.
    /// </summary>
    public interface IDataAdapterConfigurationProvider : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the read-write configuration presenter control.
        /// </summary>
        UserControl Presenter { get; }

        /// <summary>
        /// Gets the read-only configuration presenter control.
        /// </summary>
        UserControl SummaryPresenter { get; }

        /// <summary>
        /// Gets the current configuration instance.
        /// </summary>
        /// <remarks>
        /// Returns null if configuration is not valid.
        /// </remarks>
        object Configuration { get; }

        /// <summary>
        /// Gets the collection of command line arguments, representing current configuration.
        /// </summary>
        IReadOnlyDictionary<string, string> CommandLineArguments { get; }

        /// <summary>
        /// Determines if current instance can provide configuration of <paramref name="configurationType" /> type.
        /// </summary>
        /// <param name="configurationType">Type of the desired configuration.</param>
        /// <returns>true if current instance can provide desired configuration; otherwise, false.</returns>
        bool CanProvide(Type configurationType);
    }
}
