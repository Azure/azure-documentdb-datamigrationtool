using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using Microsoft.Azure.Cosmos;
using Microsoft.DataTransfer.Interfaces;
using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.DataTransfer.CosmosExtension
{
    [Export(typeof(IDataSourceExtension))]
    public class CosmosDataSourceExtension : IDataSourceExtension
    {
        public string DisplayName => "Cosmos";

        public async IAsyncEnumerable<IDataItem> ReadAsync(IConfiguration config, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var settings = config.Get<CosmosSourceSettings>();
            settings.Validate();

            var client = new CosmosClient(settings.ConnectionString,
                new CosmosClientOptions
                {
                    ConnectionMode = settings.ConnectionMode,
                    AllowBulkExecution = true
                });

            var container = client.GetContainer(settings.Database, settings.Container);
            var requestOptions = new QueryRequestOptions();
            if (!string.IsNullOrEmpty(settings.PartitionKey))
            {
                requestOptions.PartitionKey = new PartitionKey(settings.PartitionKey);
            }

            Console.WriteLine($"Reading from {settings.Database}.{settings.Container}");
            using FeedIterator<Dictionary<string, object?>> feedIterator = GetFeedIterator<Dictionary<string, object?>>(settings, container, requestOptions);
            while (feedIterator.HasMoreResults)
            {
                foreach (var item in await feedIterator.ReadNextAsync(cancellationToken))
                {
                    if (!settings.IncludeMetadataFields)
                    {
                        var corePropertiesOnly = new Dictionary<string, object?>(item.Where(kvp => !kvp.Key.StartsWith("_")));
                        yield return new CosmosDictionaryDataItem(corePropertiesOnly);
                    }
                    else
                    {
                        yield return new CosmosDictionaryDataItem(item);
                    }
                }
            }
        }

        private static FeedIterator<T> GetFeedIterator<T>(CosmosSourceSettings settings, Container container, QueryRequestOptions requestOptions)
        {
            if (string.IsNullOrWhiteSpace(settings.Query))
            {
                return container.GetItemQueryIterator<T>(requestOptions: requestOptions);
            }

            return container.GetItemQueryIterator<T>(settings.Query, requestOptions: requestOptions);
        }
    }
}