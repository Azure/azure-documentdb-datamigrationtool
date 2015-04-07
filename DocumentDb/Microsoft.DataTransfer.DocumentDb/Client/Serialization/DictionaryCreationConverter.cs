using Microsoft.DataTransfer.Basics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Client.Serialization
{
    sealed class DictionaryCreationConverter : CustomCreationConverter<Dictionary<string, object>>
    {
        public override Dictionary<string, object> Create(Type objectType)
        {
            return new Dictionary<string, object>();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(object) || base.CanConvert(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Guard.NotNull("reader", reader);
            Guard.NotNull("serializer", serializer);

            if (reader.TokenType == JsonToken.StartObject || reader.TokenType == JsonToken.Null)
                return base.ReadJson(reader, objectType, existingValue, serializer);

            if (reader.TokenType == JsonToken.StartArray)
                return serializer.Deserialize<object[]>(reader);

            return serializer.Deserialize(reader);
        }
    }
}
