using Azure.Data.Tables;
using Microsoft.DataTransfer.Interfaces;

namespace Microsoft.DataTransfer.AzureTableAPIExtension.Data
{
    public static class AzureTableAPIDataItemExtensions
    {
        public static TableEntity ToTableEntity(this IDataItem item, string? PartitionKeyFieldName, string? RowKeyFieldName)
        {
            var entity = new TableEntity();

            var partitionKeyFieldNameToUse = "PartitionKey";
            if (!string.IsNullOrWhiteSpace(PartitionKeyFieldName))
            {
                partitionKeyFieldNameToUse = PartitionKeyFieldName;
            }

            var rowKeyFieldNameToUse = "RowKey";
            if (!string.IsNullOrWhiteSpace(RowKeyFieldName))
            {
                rowKeyFieldNameToUse = RowKeyFieldName;
            }

            foreach (var key in item.GetFieldNames())
            {
                if (key.Equals(partitionKeyFieldNameToUse, StringComparison.InvariantCultureIgnoreCase))
                {
                    var partitionKey = item.GetValue(key)?.ToString();
                    entity.PartitionKey = partitionKey;
                }
                else if (key.Equals(rowKeyFieldNameToUse, StringComparison.InvariantCultureIgnoreCase))
                {
                    var rowKey = item.GetValue(key)?.ToString();
                    entity.RowKey = rowKey;
                }
                else
                {
                    entity.Add(key, item.GetValue(key));
                }
            }

            return entity;
        }
    }
}
