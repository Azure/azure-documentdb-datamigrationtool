using Azure.Data.Tables;
using Microsoft.DataTransfer.Interfaces;

namespace Microsoft.DataTransfer.AzureTableAPIExtension.Data
{
    public class AzureTableAPIDataItem : IDataItem
    {
        public AzureTableAPIDataItem(TableEntity entity)
        {
            Entity = entity;
        }

        public TableEntity Entity { get; private set; }


        public IEnumerable<string> GetFieldNames()
        {
            return Entity.Keys;
        }

        public object? GetValue(string fieldName)
        {
            try
            {
                return Entity[fieldName];
            }
            catch (Exception ex)
            {
                throw new AzureTableAPIException($"Error parsing field '${fieldName}'", ex);
            }
        }
    }
}