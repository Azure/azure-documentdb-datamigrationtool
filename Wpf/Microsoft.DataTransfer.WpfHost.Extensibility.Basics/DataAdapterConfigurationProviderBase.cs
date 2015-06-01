using Microsoft.DataTransfer.WpfHost.Basics;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace Microsoft.DataTransfer.WpfHost.Extensibility.Basics
{
    /// <summary>
    /// Provides basic functionality of <see cref="IDataAdapterConfigurationProvider" />.
    /// </summary>
    /// <typeparam name="TConfiguration">Type of the configuration that this instance can provide.</typeparam>
    public abstract class DataAdapterConfigurationProviderBase<TConfiguration> : BindableBase, IDataAdapterConfigurationProvider
    {
        private Lazy<UserControl> presenter;
        private Lazy<UserControl> summaryPresenter;
        private object configuration;

        private Lazy<TConfiguration> configurationInstance;

        /// <summary>
        /// Gets the read-write configuration presenter control.
        /// </summary>
        public UserControl Presenter
        {
            get { return presenter.Value; }
        }

        /// <summary>
        /// Gets the read-only configuration presenter control.
        /// </summary>
        public UserControl SummaryPresenter
        {
            get { return summaryPresenter.Value; }
        }

        /// <summary>
        /// Gets the current configuration instance.
        /// </summary>
        /// <remarks>
        /// Returns null if configuration is not valid.
        /// </remarks>
        public object Configuration
        {
            get { return configuration; }
            protected set { SetProperty(ref configuration, value); }
        }

        /// <summary>
        /// Gets the collection of command line arguments, representing current configuration.
        /// </summary>
        public IReadOnlyDictionary<string, string> CommandLineArguments
        {
            get { return GetCommandLineArguments(); }
        }

        /// <summary>
        /// Creates a new instance of <see cref="DataAdapterConfigurationProviderBase{TConfiguration}" />.
        /// </summary>
        protected DataAdapterConfigurationProviderBase()
        {
            presenter = new Lazy<UserControl>(CreatePresenter, true);
            summaryPresenter = new Lazy<UserControl>(CreateSummaryPresenter, true);
            configurationInstance = new Lazy<TConfiguration>(CreateConfiguration, true);
        }

        /// <summary>
        /// Determines if current instance can provide configuration of <paramref name="configurationType" /> type.
        /// </summary>
        /// <param name="configurationType">Type of the desired configuration.</param>
        /// <returns>true if current instance can provide desired configuration; otherwise, false.</returns>
        public virtual bool CanProvide(Type configurationType)
        {
            return configurationType != null &&
                configurationType.IsAssignableFrom(configurationInstance.Value.GetType());
        }

        private UserControl CreatePresenter()
        {
            return CreatePresenter(configurationInstance.Value);
        }

        private UserControl CreateSummaryPresenter()
        {
            return CreateSummaryPresenter(configurationInstance.Value);
        }

        private IReadOnlyDictionary<string, string> GetCommandLineArguments()
        {
            var arguments = new Dictionary<string, string>();
            PopulateCommandLineArguments(configurationInstance.Value, arguments);
            return arguments;
        }

        /// <summary>
        /// Creates a new instance of read-write configuration presenter control.
        /// </summary>
        /// <param name="configuration">Current configuration.</param>
        /// <returns>Read-write configuration presenter control</returns>
        protected abstract UserControl CreatePresenter(TConfiguration configuration);

        /// <summary>
        /// Creates a new instance of read-only configuration presenter control.
        /// </summary>
        /// <param name="configuration">Current configuration.</param>
        /// <returns>Read-only configuration presenter control</returns>
        protected abstract UserControl CreateSummaryPresenter(TConfiguration configuration);

        /// <summary>
        /// Creates a new instance of the configuration.
        /// </summary>
        /// <returns>New instance of the configuration.</returns>
        protected abstract TConfiguration CreateConfiguration();

        /// <summary>
        /// Populates provided collection with command line arguments that represent provided configuration.
        /// </summary>
        /// <param name="configuration">Source configuration.</param>
        /// <param name="arguments">Command line arguments collection to populate.</param>
        protected abstract void PopulateCommandLineArguments(TConfiguration configuration, IDictionary<string, string> arguments);

        /// <summary>
        /// Converts the <paramref name="collection" /> of <see cref="String" /> to a single command line argument.
        /// </summary>
        /// <param name="collection">Source collection to convert.</param>
        /// <returns><see cref="String" /> that represents command line argument value.</returns>
        protected static string AsCollectionArgument(IEnumerable<string> collection)
        {
            return String.Join(";", collection.Select(f => Regex.Replace(f, @"[;|\\]", @"\$0")));
        }
    }
}
