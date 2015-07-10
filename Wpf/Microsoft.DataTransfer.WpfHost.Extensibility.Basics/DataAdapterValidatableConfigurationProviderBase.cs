using Microsoft.DataTransfer.WpfHost.Basics;
using System.ComponentModel;

namespace Microsoft.DataTransfer.WpfHost.Extensibility.Basics
{
    /// <summary>
    /// Provides basic functionality of <see cref="IDataAdapterConfigurationProvider" /> and
    /// handles <see cref="IDataAdapterConfigurationProvider.Configuration"/> property value based on configuration validity.
    /// </summary>
    /// <typeparam name="TConfiguration">Type of the configuration that this instance can provide.</typeparam>
    public abstract class DataAdapterValidatableConfigurationProviderBase<TConfiguration> : DataAdapterConfigurationProviderBase<TConfiguration>
        where TConfiguration : ValidatableBindableBase
    {
        /// <summary>
        /// Creates new instance of the configuration.
        /// </summary>
        /// <returns>New instance of the configuration.</returns>
        protected sealed override TConfiguration CreateConfiguration()
        {
            var configuration = CreateValidatableConfiguration();
            configuration.ErrorsChanged += ConfigurationErrorsChanged;
            configuration.Validate();
            return configuration;
        }

        /// <summary>
        /// Creates new instance of the validatable configuration.
        /// </summary>
        /// <returns>New instance of the validatable configuration.</returns>
        protected abstract TConfiguration CreateValidatableConfiguration();

        private void ConfigurationErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            var configuration = sender as INotifyDataErrorInfo;

            if (configuration == null)
                return;

            Configuration = configuration.HasErrors ? null : configuration;
        }
    }
}
