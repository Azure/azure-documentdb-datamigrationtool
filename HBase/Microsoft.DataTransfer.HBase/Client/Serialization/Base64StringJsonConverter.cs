using Microsoft.DataTransfer.Basics;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Microsoft.DataTransfer.HBase.Client.Serialization
{
    sealed class Base64StringJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(string).Equals(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Guard.NotNull("reader", reader);

            var value = reader.Value as string;
            return String.IsNullOrEmpty(value) ? null
                : Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Guard.NotNull("writer", writer);

            if (value == null)
                writer.WriteNull();

            writer.WriteValue(Convert.ToBase64String(Encoding.UTF8.GetBytes((string)value)));
        }
    }
}
