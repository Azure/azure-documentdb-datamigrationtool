using System.Text.Json;
using Microsoft.DataTransfer.Interfaces;

namespace Microsoft.DataTransfer.JsonExtension
{
    public class JsonDictionaryDataItem : IDataItem
    {
        public IDictionary<string, object?> Items { get; }

        public JsonDictionaryDataItem(IDictionary<string, object?> items)
        {
            Items = items;
        }

        public IEnumerable<string> GetFieldNames()
        {
            return Items.Keys;
        }

        public object? GetValue(string fieldName)
        {
            if (!Items.TryGetValue(fieldName, out var value))
            {
                return null;
            }

            if (value is JsonElement element)
            {
                return GetElementValue(element);
            }
            return value;
        }

        private static object? GetElementValue(JsonElement element)
        {
            JsonValueKind kind = element.ValueKind;
            switch (kind)
            {
                case JsonValueKind.String:
                    return element.GetString();
                case JsonValueKind.Number:
                    return element.GetDouble();
                case JsonValueKind.True:
                    return true;
                case JsonValueKind.False:
                    return false;
                case JsonValueKind.Object:
                    return GetChildObject(element);
                case JsonValueKind.Array:
                    return element.EnumerateArray().Select(GetElementValue).ToList();
            }
            return element.GetRawText();
        }

        private static JsonDictionaryDataItem GetChildObject(JsonElement element)
        {
            return new JsonDictionaryDataItem(element.EnumerateObject().ToDictionary(p => p.Name, p => (object?)p.Value));
        }
    }
}