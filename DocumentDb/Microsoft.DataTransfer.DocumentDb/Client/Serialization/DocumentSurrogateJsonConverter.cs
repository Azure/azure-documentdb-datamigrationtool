using Microsoft.DataTransfer.Basics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Client.Serialization
{
    sealed class DocumentSurrogateJsonConverter : JsonConverter
    {
        private static readonly JsonSerializer DictionarySerializer =
            JsonSerializer.CreateDefault(new JsonSerializerSettings { Converters = { new DictionaryCreationConverter() }, MetadataPropertyHandling = MetadataPropertyHandling.Ignore });

        public override bool CanConvert(Type objectType)
        {
            Guard.NotNull("objectType", objectType);
            return objectType.IsAssignableFrom(typeof(DocumentSurrogate));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return new DocumentSurrogate(DictionarySerializer.Deserialize<Dictionary<string, object>>(reader));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var surrogate = value as DocumentSurrogate;
            if (surrogate != null)
                value = surrogate.Properties;

            Guard.NotNull("serializer", serializer);

            serializer.Serialize(writer, value);
        }
    }
}
