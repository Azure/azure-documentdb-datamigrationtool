using Microsoft.DataTransfer.Basics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Microsoft.DataTransfer.WpfHost.Basics
{
    /// <summary>
    /// Provides basic functionality of <see cref="INotifyPropertyChanged" /> and <see cref="INotifyDataErrorInfo" />.
    /// </summary>
    public abstract class ValidatableBindableBase : BindableBase, INotifyDataErrorInfo
    {
        private Dictionary<string, IReadOnlyCollection<string>> errors;

        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the entire entity.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Gets a value that indicates whether the entity has validation errors.
        /// </summary>
        public bool HasErrors
        {
            get { return errors.Values.Any(e => e != null && e.Any()); }
        }

        /// <summary>
        /// Creates a new instance of <see cref="ValidatableBindableBase" />.
        /// </summary>
        protected ValidatableBindableBase()
        {
            errors = new Dictionary<string, IReadOnlyCollection<string>>();
        }

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property to retrieve validation errors for; or <see cref="String.Empty"/>,
        /// to retrieve entity-level errors.
        /// </param>
        /// <returns>The validation errors for the property or entity.</returns>
        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName == null)
                return null;

            IReadOnlyCollection<string> propertyErrors = null;
            if (errors.TryGetValue(propertyName, out propertyErrors))
                return propertyErrors;

            return null;
        }

        /// <summary>
        /// Updates property value.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Storage of the property value.</param>
        /// <param name="value">New value for the property.</param>
        /// <param name="validator">Validation function to apply when updating the value.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected void SetProperty<T>(ref T storage, T value, Func<T, IReadOnlyCollection<string>> validator, 
            [CallerMemberName] string propertyName = null)
        {
            Guard.NotNull("validator", validator);

            SetErrors(propertyName, validator(value));
            SetProperty<T>(ref storage, value, propertyName);
        }

        /// <summary>
        /// Updates errors collection for the specified property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="activeErrors">List of errors to set for the property.</param>
        protected void SetErrors(string propertyName, IReadOnlyCollection<string> activeErrors)
        {
            if (activeErrors == null)
            {
                errors.Remove(propertyName);
            }
            else
            {
                errors[propertyName] = activeErrors;
            }

            // Little trick here - raise the event despite of the changes, this allows to force validation by assigning property value back to itself
            OnErrorsChanged(propertyName);
        }

        /// <summary>
        /// Notifies all subscribers that property errors has been changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnErrorsChanged(string propertyName)
        {
            var handler = ErrorsChanged;
            if (handler != null)
                handler(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
