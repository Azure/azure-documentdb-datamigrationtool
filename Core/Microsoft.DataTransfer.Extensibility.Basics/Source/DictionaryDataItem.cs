using Microsoft.DataTransfer.Basics;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.Extensibility.Basics.Source
{
    /// <summary>
    /// Represents data artifact that encapsulates collection of fields in a form of
    /// <see cref="System.String" />-<see cref="System.Object" /> dictionary.
    /// </summary>
    public class DictionaryDataItem : IDataItem
    {
        private IReadOnlyDictionary<string, object> fields;

        /// <summary>
        /// Creates a new instance of <see cref="DictionaryDataItem" />.
        /// </summary>
        /// <param name="fields">Encapsulated fields collection.</param>
        public DictionaryDataItem(IReadOnlyDictionary<string, object> fields)
        {
            Guard.NotNull("fields", fields);
            this.fields = fields;
        }

        /// <summary>
        /// Provides collection of field names available in the data artifact.
        /// </summary>
        /// <returns>Collection of field names.</returns>
        public IEnumerable<string> GetFieldNames()
        {
            return fields.Keys;
        }

        /// <summary>
        /// Provides a value of the specified data artifact field.
        /// </summary>
        /// <param name="fieldName">Name of data artifact field.</param>
        /// <returns>Value of the field.</returns>
        public object GetValue(string fieldName)
        {
            Guard.NotNull("fieldName", fieldName);

            object value;
            if (!fields.TryGetValue(fieldName, out value))
                throw CommonErrors.DataItemFieldNotFound(fieldName);

            if (value is IReadOnlyDictionary<string, object>)
                return new DictionaryDataItem((IReadOnlyDictionary<string, object>)value);

            return value;
        }
    }
}
