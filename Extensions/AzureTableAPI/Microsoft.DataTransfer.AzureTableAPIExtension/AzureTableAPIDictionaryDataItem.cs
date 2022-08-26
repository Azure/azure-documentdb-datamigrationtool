using System.Text.Json;
using Microsoft.DataTransfer.Interfaces;

namespace Microsoft.DataTransfer.AzureTableAPIExtension
{
    public class AzureTableAPIDictionaryDataItem : IDataItem
    {
        public IDictionary<string, object?> Items { get; }

        public AzureTableAPIDictionaryDataItem(IDictionary<string, object?> items)
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
                        return element.EnumerateArray().Select(GetChildObject).ToList();
                }
                return element.GetRawText();
            }
            return value;
        }

        private static AzureTableAPIDictionaryDataItem GetChildObject(JsonElement element)
        {
            return new AzureTableAPIDictionaryDataItem(element.EnumerateObject().ToDictionary(p => p.Name, p => (object?)p.Value));
        }
    }
}