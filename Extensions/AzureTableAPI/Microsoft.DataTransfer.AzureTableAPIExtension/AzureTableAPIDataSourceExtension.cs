using Azure;
using Azure.Data.Tables;
using Microsoft.DataTransfer.AzureTableAPIExtension.Data;
using Microsoft.DataTransfer.AzureTableAPIExtension.Settings;
using Microsoft.DataTransfer.Interfaces;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;

namespace Microsoft.DataTransfer.AzureTableAPIExtension
{
    [Export(typeof(IDataSourceExtension))]
    public class AzureTableAPIDataSourceExtension : IDataSourceExtension
    {
        public string DisplayName => "AzureTableAPI";

        public async IAsyncEnumerable<IDataItem> ReadAsync(IConfiguration config, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var settings = config.Get<AzureTableAPIDataSourceSettings>();
            settings.Validate();

            var serviceClient = new TableServiceClient(settings.ConnectionString);
            var tableClient = serviceClient.GetTableClient(settings.Table);

            //Pageable<TableEntity> queryResultsFilter = tableClient.Query<TableEntity>(filter: $"PartitionKey eq '{partitionKey}'");
            AsyncPageable<TableEntity> queryResults;
            if (!string.IsNullOrWhiteSpace(settings.QueryFilter)) {
                queryResults = tableClient.QueryAsync<TableEntity>(filter: settings.QueryFilter);
            } else {
                queryResults = tableClient.QueryAsync<TableEntity>();
            }

            var enumerator = queryResults.GetAsyncEnumerator();
            while (await enumerator.MoveNextAsync())
            {
                yield return new AzureTableAPIDataItem(enumerator.Current, settings.PartitionKeyFieldName, settings.RowKeyFieldName);
            }
            //do
            //{
            //    yield return new AzureTableAPIDataItem(enumerator.Current, settings.PartitionKeyFieldName, settings.RowKeyFieldName);
            //} while (await enumerator.MoveNextAsync());
        }
    }
}