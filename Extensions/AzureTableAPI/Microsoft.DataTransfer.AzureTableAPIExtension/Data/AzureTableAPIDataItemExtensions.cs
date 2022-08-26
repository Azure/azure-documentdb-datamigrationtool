using Azure.Data.Tables;
using Microsoft.DataTransfer.Interfaces;

namespace Microsoft.DataTransfer.AzureTableAPIExtension.Data
{
    public static class AzureTableAPIDataItemExtensions
    {
        public static TableEntity ToTableEntity(this IDataItem item)
        {
            var entity = new TableEntity();

            foreach (var key in item.GetFieldNames())
            {
                entity.Add(key, item.GetValue(key));
            }

            return entity;
        }
    }
}
