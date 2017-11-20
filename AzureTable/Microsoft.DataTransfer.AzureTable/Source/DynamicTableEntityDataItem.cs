using Microsoft.Azure.CosmosDB.Table;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
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

        public DynamicTableEntity GetDynamicTableEntity()
        {
            return data;
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
