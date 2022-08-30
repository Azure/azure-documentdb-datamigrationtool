using Azure.Data.Tables;
using Microsoft.DataTransfer.Interfaces;

namespace Microsoft.DataTransfer.AzureTableAPIExtension.Data
{
    public class AzureTableAPIDataItem : IDataItem
    {
        public AzureTableAPIDataItem(TableEntity entity, string? partitionKeyFieldName, string? rowKeyFieldName)
        {
            this.Entity = entity;
            this.PartitionKeyFieldName = partitionKeyFieldName;
            this.RowKeyFieldName = rowKeyFieldName;
        }

        public TableEntity Entity { get; private set; }
        public string? PartitionKeyFieldName { get; private set; }
        public string? RowKeyFieldName { get; private set; }



        public IEnumerable<string> GetFieldNames()
        {
            var keys = Entity.Keys.ToList();
            
            if (!string.IsNullOrWhiteSpace(this.PartitionKeyFieldName))
            {
                keys.Remove("PartitionKey");
                keys.Add(this.PartitionKeyFieldName);
            }

            if (!string.IsNullOrWhiteSpace(this.RowKeyFieldName))
            {
                keys.Remove("RowKey");
                keys.Add(this.RowKeyFieldName);
            }

            return Entity.Keys;
        }

        public object? GetValue(string fieldName)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.PartitionKeyFieldName) && fieldName.Equals(this.PartitionKeyFieldName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return Entity.PartitionKey;
                }
                else if (!string.IsNullOrWhiteSpace(this.RowKeyFieldName) && fieldName.Equals(this.RowKeyFieldName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return Entity.RowKey;
                }

                return Entity[fieldName];
            }
            catch (Exception ex)
            {
                throw new AzureTableAPIException($"Error parsing field '${fieldName}'", ex);
            }
        }
    }
}