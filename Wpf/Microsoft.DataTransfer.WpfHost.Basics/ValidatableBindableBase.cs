using Microsoft.DataTransfer.Basics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Microsoft.DataTransfer.WpfHost.Basics
{
    /// <summary>
    /// Provides basic functionality of <see cref="INotifyPropertyChanged" /> and <see cref="INotifyDataErrorInfo" />.
    /// </summary>
    public abstract class ValidatableBindableBase : BindableBase, INotifyDataErrorInfo
    {
        private static readonly BindingFlags FlatPropertiesBinding = BindingFlags.Instance | BindingFlags.FlattenHierarchy |
            BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty;

        private Dictionary<string, IReadOnlyCollection<string>> errors;

        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the entire entity.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Gets a value that indicates whether the entity has validation errors.
        /// </summary>
        public virtual bool HasErrors
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
        /// Forces validation on the current instance.
        /// </summary>
        public virtual void Validate()
        {
            foreach (var property in GetType().GetProperties(FlatPropertiesBinding))
                if (property.GetMethod != null && property.SetMethod != null)
                    property.SetValue(this, property.GetValue(this));
        }

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property to retrieve validation errors for; or <see cref="String.Empty"/>,
        /// to retrieve entity-level errors.
        /// </param>
        /// <returns>The validation errors for the property or entity.</returns>
        public virtual IEnumerable GetErrors(string propertyName)
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
        /// <returns>true if storage value was changed; otherwise, false.</returns>
        protected bool SetProperty<T>(ref T storage, T value, Func<T, IReadOnlyCollection<string>> validator, 
            [CallerMemberName] string propertyName = null)
        {
            Guard.NotNull("validator", validator);

            SetErrors(propertyName, validator(value));
            return SetProperty<T>(ref storage, value, propertyName);
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

        #region Validators

        /// <summary>
        /// Verifies that provided <paramref name="value" /> is not an empty string.
        /// </summary>
        /// <param name="value">String to validate.</param>
        /// <returns>Collection of validation errors if <paramref name="value" /> is an empty string; otherwise, null.</returns>
        protected static IReadOnlyCollection<string> ValidateNonEmptyString(string value)
        {
            return String.IsNullOrEmpty(value) ? new[] { Resources.NonEmptyStringRequired } : null;
        }

        /// <summary>
        /// Verifies that provided <paramref name="value" /> is not an empty collection.
        /// </summary>
        /// <typeparam name="T">Type of the collection element.</typeparam>
        /// <param name="value">Collection to validate.</param>
        /// <returns>Collection of validation errors if <paramref name="value" /> is an empty collection; otherwise, null.</returns>
        protected static IReadOnlyCollection<string> ValidateNonEmptyCollection<T>(IEnumerable<T> value)
        {
            return value == null || !value.Any() ? new[] { Resources.NonEmptyCollectionRequired } : null;
        }

        /// <summary>
        /// Verifies that provided <paramref name="value" /> is greater than zero.
        /// </summary>
        /// <param name="value">Number to validate.</param>
        /// <returns>Collection of validation errors if <paramref name="value" /> is less or equal to zero; otherwise, null.</returns>
        protected static IReadOnlyCollection<string> ValidatePositiveInteger(int? value)
        {
            return value > 0 ? null : new[] { Resources.PositiveNumberRequired };
        }

        /// <summary>
        /// Verifies that provided <paramref name="value" /> is greater or equal to zero.
        /// </summary>
        /// <param name="value">Number to validate.</param>
        /// <returns>Collection of validation errors if <paramref name="value" /> is less than zero; otherwise, null.</returns>
        protected static IReadOnlyCollection<string> ValidateNonNegativeInteger(int? value)
        {
            return value >= 0 ? null : new[] { Resources.NonNegativeNumberRequired };
        }

        #endregion Validators
    }
}
