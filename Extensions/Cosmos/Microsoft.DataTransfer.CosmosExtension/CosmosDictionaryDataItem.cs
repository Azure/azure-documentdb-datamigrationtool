using System.Text.Json;
using Microsoft.DataTransfer.Interfaces;
using Newtonsoft.Json.Linq;

namespace Microsoft.DataTransfer.CosmosExtension
{
    public class CosmosDictionaryDataItem : IDataItem
    {
        public IDictionary<string, object?> Items { get; }

        public CosmosDictionaryDataItem(IDictionary<string, object?> items)
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

            return GetChildObject(value);
        }

        private static object? GetChildObject(object? value)
        {
            if (value is JObject element)
            {
                return new CosmosDictionaryDataItem(element.ToObject<IDictionary<string, object?>>().ToDictionary(k => k.Key, v => v.Value));
            }
            if (value is JArray array)
            {
                return array.ToObject<List<object?>>().Select(GetChildObject).ToList();
            }

            return value;
        }
    }
}