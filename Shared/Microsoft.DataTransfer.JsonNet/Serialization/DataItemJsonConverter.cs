using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Microsoft.DataTransfer.JsonNet.Serialization
{
    /// <summary>
    /// Converts <see cref="IDataItem" /> to JSON.
    /// </summary>
    public sealed class DataItemJsonConverter : JsonConverter
    {
        /// <summary>
        /// Singleton instance of the converter.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes",
            Justification = "Immutable singleton instance")]
        public static readonly DataItemJsonConverter Instance = new DataItemJsonConverter();

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>true if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType)
        {
            Guard.NotNull("objectType", objectType);
            return typeof(IDataItem).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Guard.NotNull("objectType", objectType);
            Guard.NotNull("serializer", serializer);

            if (!objectType.IsAssignableFrom(typeof(JObjectDataItem)))
                return serializer.Deserialize(reader, objectType);

            return new JObjectDataItem(serializer.Deserialize<JObject>(reader));
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Guard.NotNull("writer", writer);
            Guard.NotNull("serializer", serializer);

            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var dataItem = (IDataItem)value;

            writer.WriteStartObject();
            foreach (var fieldName in dataItem.GetFieldNames())
            {
                writer.WritePropertyName(fieldName);
                serializer.Serialize(writer, dataItem.GetValue(fieldName));
            }
            writer.WriteEndObject();
        }
    }
}
