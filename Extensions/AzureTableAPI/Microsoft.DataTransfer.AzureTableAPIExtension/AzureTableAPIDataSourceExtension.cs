using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
// using System.Text.Json;
using Microsoft.DataTransfer.Interfaces;
using Microsoft.Extensions.Configuration;
using Azure;

namespace Microsoft.DataTransfer.AzureTableAPIExtension
{
    [Export(typeof(IDataSourceExtension))]
    public class AzureTableAPIDataSourceExtension : IDataSourceExtension
    {
        public string DisplayName => "AzureTableAPI";
        public async IAsyncEnumerable<IDataItem> ReadAsync(IConfiguration config, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var settings = config.Get<AzureTableAPISourceSettings>();
            settings.Validate();

            var serviceClient = new TableServiceClient(settings.ConnectionString);
            var tableClient = serviceClient.GetTableClient(settings.Table);

            //Pageable<TableEntity> queryResultsFilter = tableClient.Query<TableEntity>(filter: $"PartitionKey eq '{partitionKey}'");
            AsyncPageable<TableEntity> queryResultsFilter;
            if (string.IsNullOrWhiteSpace(settings.QueryFilter)) {
                queryResultsFilter = await tableClient.QueryAsync<TableEntity>(filter: settings.QueryFilter);
            } else {
                queryResultsFilter = await tableClient.QueryAsync<TableEntity>();
            }

            foreach (TableEntity entity in queryResultsFilter)
            {
                Console.WriteLine($"{entity.GetString("Product")}: {entity.GetDouble("Price")}");
            }
            // if (settings.FilePath != null)
            // {
            //     await using var file = File.OpenRead(settings.FilePath);
            //     var list = await JsonSerializer.DeserializeAsync<List<Dictionary<string, object?>>>(file, cancellationToken: cancellationToken);

            //     if (list != null)
            //     {
            //         foreach (var listItem in list)
            //         {
            //             yield return new AzureTableAPIDictionaryDataItem(listItem);
            //         }
            //     }
            // }

        }
    }
}