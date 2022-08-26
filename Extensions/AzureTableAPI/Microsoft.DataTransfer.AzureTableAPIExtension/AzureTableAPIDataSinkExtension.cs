using Azure.Data.Tables;
using Microsoft.DataTransfer.AzureTableAPIExtension.Data;
using Microsoft.DataTransfer.AzureTableAPIExtension.Settings;
using Microsoft.DataTransfer.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Microsoft.DataTransfer.AzureTableAPIExtension
{
    public class AzureTableAPIDataSinkExtension : IDataSinkExtension
    {
        public string DisplayName => "AzureTableAPI";

        public async Task WriteAsync(IAsyncEnumerable<IDataItem> dataItems, IConfiguration config, CancellationToken cancellationToken = default)
        {
            var settings = config.Get<AzureTableAPIDataSinkSettings>();
            settings.Validate();

            var serviceClient = new TableServiceClient(settings.ConnectionString);
            var tableClient = serviceClient.GetTableClient(settings.Table);

            await tableClient.CreateIfNotExistsAsync();

            await foreach(var item in dataItems)
            {
                var entity = item.ToTableEntity(settings.PartitionKeyFieldName, settings.RowKeyFieldName);
                await tableClient.AddEntityAsync(entity);
            }
        }
    }
}
