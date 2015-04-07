using Microsoft.DataTransfer.WpfHost.Basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.DataTransfer.WpfHost.Extensibility.Basics
{
    /// <summary>
    /// Provides basic functionality for configuration that supports validation.
    /// </summary>
    public abstract class ValidatableConfiguration : ValidatableBindableBase
    {
        private static readonly BindingFlags FlatPropertiesBinding = BindingFlags.Instance | BindingFlags.FlattenHierarchy |
            BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty;

        /// <summary>
        /// Forces validation on the current configuration instance.
        /// </summary>
        public virtual void Validate()
        {
            foreach (var property in GetType().GetProperties(FlatPropertiesBinding))
                if (property.GetMethod != null && property.SetMethod != null)
                    property.SetValue(this, property.GetValue(this));
        }

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
    }
}
