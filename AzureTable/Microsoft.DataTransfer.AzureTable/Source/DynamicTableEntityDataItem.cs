using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.AzureTable.Source
{
    sealed class DynamicTableEntityDataItem : IDataItem
    {
        private readonly DynamicTableEntity data;

        public DynamicTableEntityDataItem(DynamicTableEntity data)
        {
            Guard.NotNull("data", data);

            this.data = data;
        }

        public IEnumerable<string> GetFieldNames()
        {
            return data.Properties.Keys;
        }

        public object GetValue(string fieldName)
        {
            Guard.NotNull("fieldName", fieldName);

            EntityProperty property;
            if (!data.Properties.TryGetValue(fieldName, out property))
                throw Errors.DataItemFieldNotFound(fieldName);

            return property.PropertyAsObject == null ? null : property.PropertyAsObject;
        }
    }
}
