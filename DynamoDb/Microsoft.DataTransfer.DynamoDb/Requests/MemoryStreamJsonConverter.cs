using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DataTransfer.DynamoDb.Requests
{
    sealed class MemoryStreamJsonConverter : JsonConverter
    {
        public static readonly MemoryStreamJsonConverter Instance = new MemoryStreamJsonConverter();

        public override bool CanConvert(Type objectType)
        {
            return typeof(MemoryStream).Equals(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (!CanConvert(objectType))
                throw UnsupportedType(objectType);

            if (reader.TokenType == JsonToken.String)
            {
                return new MemoryStream(Convert.FromBase64String((string)reader.Value));
            }
            else if (reader.TokenType == JsonToken.StartArray)
            {
                return new MemoryStream(serializer.Deserialize<IEnumerable<int>>(reader).Select(i => (byte)i).ToArray());
            }

            throw Errors.UnsupportedBinaryFormat(Enum.GetName(typeof(JsonToken), reader.TokenType));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
                writer.WriteNull();

            var stream = value as MemoryStream;
            if (stream == null)
                throw UnsupportedType(value.GetType());

            serializer.Serialize(writer, stream.ToArray());
        }

        private static Exception UnsupportedType(Type type)
        {
            return Errors.TypeConversionNotSupported(type, typeof(MemoryStreamJsonConverter));
        }
    }
}
