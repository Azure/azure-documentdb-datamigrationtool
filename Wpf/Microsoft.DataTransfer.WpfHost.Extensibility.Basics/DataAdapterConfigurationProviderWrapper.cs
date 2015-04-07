using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.WpfHost.Extensibility.Basics
{
    /// <summary>
    /// Encapsulates instance of <see cref="IDataAdapterConfigurationProvider" /> to hide implementation details.
    /// </summary>
    public class DataAdapterConfigurationProviderWrapper : IDataAdapterConfigurationProvider
    {
        /// <summary>
        /// Gets the encapsulated <see cref="IDataAdapterConfigurationProvider" /> instance.
        /// </summary>
        protected IDataAdapterConfigurationProvider Provider { get; private set; }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { Provider.PropertyChanged += value; }
            remove { Provider.PropertyChanged -= value; }
        }

        /// <summary>
        /// Gets the read-write configuration presenter control.
        /// </summary>
        public UserControl Presenter
        {
            get { return Provider.Presenter; }
        }

        /// <summary>
        /// Gets the read-only configuration presenter control.
        /// </summary>
        public UserControl SummaryPresenter
        {
            get { return Provider.SummaryPresenter; }
        }

        /// <summary>
        /// Gets the current configuration instance.
        /// </summary>
        /// <remarks>
        /// Returns null if configuration is not valid.
        /// </remarks>
        public object Configuration
        {
            get { return Provider.Configuration; }
        }

        /// <summary>
        /// Gets the collection of command line arguments, representing current configuration.
        /// </summary>
        public IReadOnlyDictionary<string, string> CommandLineArguments
        {
            get { return Provider.CommandLineArguments; }
        }

        /// <summary>
        /// Creates a new instance of <see cref="DataAdapterConfigurationProviderWrapper" />.
        /// </summary>
        /// <param name="provider"></param>
        public DataAdapterConfigurationProviderWrapper(IDataAdapterConfigurationProvider provider)
        {
            Provider = provider;
        }

        /// <summary>
        /// Determines if current instance can provide configuration of <paramref name="configurationType" /> type.
        /// </summary>
        /// <param name="configurationType">Type of the desired configuration.</param>
        /// <returns>true if current instance can provide desired configuration; otherwise, false.</returns>
        public bool CanProvide(Type configurationType)
        {
            return Provider.CanProvide(configurationType);
        }
    }
}
