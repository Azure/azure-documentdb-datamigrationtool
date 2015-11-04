using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.JsonNet.Serialization;
using Newtonsoft.Json;
using System;

namespace Microsoft.DataTransfer.DocumentDb.Sink
{
    sealed class DataItemSurrogateJsonConverter : JsonConverter
    {
        private static readonly JsonSerializer DataItemSerializer =
            JsonSerializer.CreateDefault(new JsonSerializerSettings
            {
                Converters =
                    {
                        DataItemJsonConverter.Instance,
                        GeoJsonConverter.Instance
                    }
            });

        public override bool CanConvert(Type objectType)
        {
            Guard.NotNull("objectType", objectType);
            return objectType.IsAssignableFrom(typeof(DataItemSurrogate));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Guard.NotNull("objectType", objectType);

            if (objectType.IsAssignableFrom(typeof(DataItemSurrogate)))
                return new DataItemSurrogate(DataItemSerializer.Deserialize<IDataItem>(reader));

            Guard.NotNull("serializer", serializer);

            return serializer.Deserialize(reader, objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var surrogate = value as DataItemSurrogate;
            if (surrogate != null)
            {
                DataItemSerializer.Serialize(writer, surrogate.DataItem);
                return;
            }

            Guard.NotNull("serializer", serializer);

            serializer.Serialize(writer, value);
        }
    }
}
