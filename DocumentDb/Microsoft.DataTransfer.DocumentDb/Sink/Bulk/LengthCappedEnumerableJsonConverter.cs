using Microsoft.DataTransfer.Basics;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    sealed class LengthCappedEnumerableJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(LengthCappedEnumerableSurrogate).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Guard.NotNull("serializer", serializer);
            return new LengthCappedEnumerableSurrogate(serializer.Deserialize<IEnumerable>(reader), 0, 0);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times",
            Justification = "As per .NET framework design guidelines Dispose method should be re-entrant")]
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Guard.NotNull("writer", writer);
            Guard.NotNull("serializer", serializer);

            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var surrogate = (LengthCappedEnumerableSurrogate)value;

            writer.WriteStartArray();

            var hasDocuments = false;
            var persistedDocuments = 0;
            var jsonText = new StringBuilder(surrogate.MaxSerializedLength);
            using (var stringWriter = new StringWriter(jsonText, CultureInfo.InvariantCulture))
            {
                var totalDocuments = 0;
                var totalLength = 1; // Add one for array start element

                foreach (var item in surrogate)
                {
                    hasDocuments = true;

                    if (++totalDocuments > surrogate.MaxElements)
                        break;

                    serializer.Serialize(stringWriter, item);

                    var jsonString = jsonText.ToString();
                    totalLength += Encoding.UTF8.GetByteCount(jsonString) + 1; // Add one for comma separator or array end element
                    if (totalLength > surrogate.MaxSerializedLength)
                        break;

                    writer.WriteRawValue(jsonString);
                    ++persistedDocuments;
                    jsonText.Clear();
                }
            }

            surrogate.LastSerializedCount = persistedDocuments;

            if (hasDocuments && persistedDocuments <= 0)
                throw Errors.DocumentSizeExceedsBulkScriptSize();

            writer.WriteEndArray();
        }
    }
}
