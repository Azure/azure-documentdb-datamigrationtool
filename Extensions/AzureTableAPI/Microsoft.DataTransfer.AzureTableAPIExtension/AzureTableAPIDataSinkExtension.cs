using Azure.Data.Tables;
using Microsoft.DataTransfer.AzureTableAPIExtension.Data;
using Microsoft.DataTransfer.AzureTableAPIExtension.Settings;
using Microsoft.DataTransfer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.Composition;

namespace Microsoft.DataTransfer.AzureTableAPIExtension
{
    [Export(typeof(IDataSinkExtension))]
    public class AzureTableAPIDataSinkExtension : IDataSinkExtension
    {
        public string DisplayName => "AzureTableAPI";

        public async Task WriteAsync(IAsyncEnumerable<IDataItem> dataItems, IConfiguration config, IDataSourceExtension dataSource, ILogger logger, CancellationToken cancellationToken = default)
        {
            var settings = config.Get<AzureTableAPIDataSinkSettings>();
            settings.Validate();

            var serviceClient = new TableServiceClient(settings.ConnectionString);
            var tableClient = serviceClient.GetTableClient(settings.Table);

            await tableClient.CreateIfNotExistsAsync();

            var createTasks = new List<Task>();
            await foreach(var item in dataItems.WithCancellation(cancellationToken))
            {
               var entity = item.ToTableEntity(settings.PartitionKeyFieldName, settings.RowKeyFieldName);
               createTasks.Add(tableClient.AddEntityAsync(entity));
            }

            await Task.WhenAll(createTasks);
        }
    }
}
