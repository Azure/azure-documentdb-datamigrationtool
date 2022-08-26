using Microsoft.DataTransfer.Interfaces;

namespace Microsoft.DataTransfer.AzureTableAPIExtension.UnitTests
{
    public class MockDataItem : IDataItem
    {
        public MockDataItem() { this.Data = new Dictionary<string, object>(); }
        public MockDataItem(Dictionary<string, object> data)
        {
            this.Data = data;
        }
        public Dictionary<string, object> Data;

        public IEnumerable<string> GetFieldNames()
        {
            return this.Data.Keys;
        }

        public object? GetValue(string fieldName)
        {
            return this.Data.GetValueOrDefault(fieldName);
        }
    }
}
