using Microsoft.DataTransfer.Basics;
using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.Extensibility.Basics.Source
{
    /// <summary>
    /// Represents data artifact that can represent nested documents by splitting property names with the given separator.
    /// </summary>
    public sealed class NestedDataItem : DictionaryDataItem
    {
        private Dictionary<string, object> fields;
        private string nestingSeparator;

        /// <summary>
        /// Creates a new instance of <see cref="NestedDataItem" /> without any data in it.
        /// </summary>
        /// <param name="nestingSeparator">Separator to use when splitting property names into nested documents.</param>
        /// <returns>New instance of <see cref="NestedDataItem" />.</returns>
        public static NestedDataItem Create(string nestingSeparator)
        {
            return new NestedDataItem(new Dictionary<string, object>(), nestingSeparator);
        }

        /// <summary>
        /// Creates a new instance of <see cref="NestedDataItem" /> without any data in it.
        /// </summary>
        /// <param name="properties">Collection of properties to initialize data artifact with.</param>
        /// <param name="nestingSeparator">Separator to use when splitting property names into nested documents.</param>
        /// <returns>New instance of <see cref="NestedDataItem" />.</returns>
        public static NestedDataItem Create(IEnumerable<KeyValuePair<string, object>> properties, string nestingSeparator)
        {
            Guard.NotNull("properties", properties);

            var dataItem = new NestedDataItem(new Dictionary<string, object>(), nestingSeparator);

            foreach (var property in properties)
                dataItem.AddProperty(property.Key, property.Value);

            return dataItem;
        }

        private NestedDataItem(Dictionary<string, object> fields, string nestingSeparator)
            : base(fields)
        {
            this.fields = fields;
            this.nestingSeparator = nestingSeparator;
        }

        /// <summary>
        /// Adds new property to the data artifact.
        /// </summary>
        /// <remarks>
        /// In case property name contains nesting separator - property will be treated as property of the sub-document.
        /// </remarks>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        public void AddProperty(string name, object value)
        {
            Guard.NotNull("name", name);

            var nestingSeparatorIndex = String.IsNullOrEmpty(nestingSeparator) ? -1 : name.IndexOf(nestingSeparator, StringComparison.Ordinal);

            if (nestingSeparatorIndex < 0)
            {
                fields[name] = value;
                return;
            }

            var parentName = name.Substring(0, nestingSeparatorIndex);

            object dataItem;
            NestedDataItem nestedDataItem;
            if (!fields.TryGetValue(parentName, out dataItem) || (nestedDataItem = dataItem as NestedDataItem) == null)
                fields[parentName] = nestedDataItem = NestedDataItem.Create(nestingSeparator);

            nestedDataItem.AddProperty(name.Substring(nestingSeparatorIndex + nestingSeparator.Length), value);
        }
    }
}
