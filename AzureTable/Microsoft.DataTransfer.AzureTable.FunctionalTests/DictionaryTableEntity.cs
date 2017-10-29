using Microsoft.Azure.CosmosDB.Table;
using Microsoft.Azure.Storage;
using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.AzureTable.FunctionalTests
{
    sealed class DictionaryTableEntity : TableEntity
    {
        private readonly IReadOnlyDictionary<string, object> data;

        public DictionaryTableEntity() { }

        public DictionaryTableEntity(string key, IReadOnlyDictionary<string, object> data)
        {
            PartitionKey = String.Empty;
            RowKey = key;
            this.data = data;
        }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var results = base.WriteEntity(operationContext);

            foreach (var item in data)
                results.Add(item.Key, AsEntityProperty(item.Value));

            return results;
        }

        private static EntityProperty AsEntityProperty(object value)
        {
            if (value is bool)
                return new EntityProperty((bool)value);

            if (value is byte[])
                return new EntityProperty((byte[])value);

            if (value is DateTime)
                return new EntityProperty((DateTime)value);

            if (value is DateTimeOffset)
                return new EntityProperty((DateTimeOffset)value);

            if (value is double || value is float)
                return new EntityProperty((double)value);

            if (value is Guid)
                return new EntityProperty((Guid)value);

            if (value is int)
                return new EntityProperty((int)value);

            if (value is long)
                return new EntityProperty((long)value);

            if (value is string)
                return new EntityProperty((string)value);

            return new EntityProperty((string)null);
        }
    }
}
