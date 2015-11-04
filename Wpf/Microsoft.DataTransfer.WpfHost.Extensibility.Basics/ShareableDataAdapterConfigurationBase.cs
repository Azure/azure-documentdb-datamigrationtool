using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Collections;
using Microsoft.DataTransfer.WpfHost.Basics;
using System;
using System.Collections;
using System.ComponentModel;

namespace Microsoft.DataTransfer.WpfHost.Extensibility.Basics
{
    /// <summary>
    /// Base class for data adapter configuration that can share one or more properties with another configurations.
    /// </summary>
    /// <typeparam name="TShared">Type of the configuration that holds shared properties.</typeparam>
    public abstract class ShareableDataAdapterConfigurationBase<TShared> : ValidatableBindableBase
        where TShared : class, INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private Lazy<Map<string, string>> sharedPropertiesMapping;

        /// <summary>
        /// Gets the mapping between properties of the shared configuration and local properties.
        /// </summary>
        protected Map<string, string> SharedPropertiesMapping
        {
            get { return sharedPropertiesMapping.Value; }
        }

        /// <summary>
        /// Gets the shared configuration.
        /// </summary>
        protected TShared SharedConfiguration { get; private set; }

        /// <summary>
        /// Gets a value that indicates whether the entity has validation errors.
        /// </summary>
        public override bool HasErrors
        {
            get { return SharedConfiguration.HasErrors || base.HasErrors; }
        }

        /// <summary>
        /// Creates a new instance of <see cref="ShareableDataAdapterConfigurationBase{T}" />.
        /// </summary>
        /// <param name="sharedConfiguration">Instance of the shared configuration.</param>
        public ShareableDataAdapterConfigurationBase(TShared sharedConfiguration)
        {
            Guard.NotNull("sharedConfiguration", sharedConfiguration);

            SharedConfiguration = sharedConfiguration;
            SharedConfiguration.PropertyChanged += SharedConfigurationPropertyChanged;
            SharedConfiguration.ErrorsChanged += SharedConfigurationErrorsChanged;

            sharedPropertiesMapping = new Lazy<Map<string, string>>(GetSharedPropertiesMapping, true);
        }

        /// <summary>
        /// Returns the mapping between properties of the shared configuration and local properties.
        /// </summary>
        /// <returns>Properties mapping.</returns>
        protected abstract Map<string, string> GetSharedPropertiesMapping();

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property to retrieve validation errors for; or <see cref="String.Empty"/>,
        /// to retrieve entity-level errors.
        /// </param>
        /// <returns>The validation errors for the property or entity.</returns>
        public override IEnumerable GetErrors(string propertyName)
        {
            string sharedPropertyName;
            if (propertyName != null && SharedPropertiesMapping.TryGetKey(propertyName, out sharedPropertyName))
                return SharedConfiguration.GetErrors(sharedPropertyName);

            return base.GetErrors(propertyName);
        }

        private void SharedConfigurationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string localPropertyName;
            if (e.PropertyName != null && SharedPropertiesMapping.TryGetValue(e.PropertyName, out localPropertyName))
                OnPropertyChanged(localPropertyName);
        }

        private void SharedConfigurationErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            string localPropertyName;
            if (e.PropertyName != null && SharedPropertiesMapping.TryGetValue(e.PropertyName, out localPropertyName))
                OnErrorsChanged(localPropertyName);
        }
    }
}
