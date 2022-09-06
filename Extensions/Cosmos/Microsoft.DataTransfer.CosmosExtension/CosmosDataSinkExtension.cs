using System.ComponentModel.Composition;
using System.Dynamic;
using Microsoft.Azure.Cosmos;
using Microsoft.DataTransfer.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Microsoft.DataTransfer.CosmosExtension
{
    [Export(typeof(IDataSinkExtension))]
    public class CosmosDataSinkExtension : IDataSinkExtension
    {
        public string DisplayName => "Cosmos";

        public async Task WriteAsync(IAsyncEnumerable<IDataItem> dataItems, IConfiguration config, CancellationToken cancellationToken = default)
        {
            var settings = config.Get<CosmosSinkSettings>();
            settings.Validate();

            var client = new CosmosClient(settings.ConnectionString,
                new CosmosClientOptions
                {
                    ConnectionMode = ConnectionMode.Gateway,
                    AllowBulkExecution = true
                });

            Database database = await client.CreateDatabaseIfNotExistsAsync(settings.Database, cancellationToken: cancellationToken);

            if (settings.RecreateContainer)
            {
                try
                {
                    await database.GetContainer(settings.Container).DeleteContainerAsync(cancellationToken: cancellationToken);
                }
                catch { }
            }

            Container? container = await database.CreateContainerIfNotExistsAsync(settings.Container, settings.PartitionKeyPath, cancellationToken: cancellationToken);

            var createTasks = new List<Task>();
            await foreach (var source in dataItems.WithCancellation(cancellationToken))
            {
                var task = InsertItem(container, source, cancellationToken);
                createTasks.Add(task);
            }

            await Task.WhenAll(createTasks);
        }

        private static async Task<ItemResponse<ExpandoObject>?> InsertItem(Container container, IDataItem? source, CancellationToken cancellationToken)
        {
            ExpandoObject? item = BuildObject(source, true);
            if (item == null)
                return null;

            try
            {
                ItemResponse<ExpandoObject> itemResponse = await container.CreateItemAsync(item, cancellationToken: cancellationToken);
                if (itemResponse.StatusCode != System.Net.HttpStatusCode.OK && itemResponse.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    // TODO: report errors
                }

                return itemResponse;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                // TODO: item already exists, use upsert instead based on config?
                return null;
            }
            catch (Exception ex)
            {
                // TODO: report errors
                return null;
            }
        }

        private static ExpandoObject? BuildObject(IDataItem? source, bool requireStringId = false)
        {
            if (source == null)
                return null;

            var fields = source.GetFieldNames();
            var item = new ExpandoObject();
            if (requireStringId && !fields.Contains("id"))
            {
                item.TryAdd("id", Guid.NewGuid().ToString());
            }
            foreach (string field in fields)
            {
                object? value = source.GetValue(field);
                if (string.Equals(field, "id", StringComparison.CurrentCultureIgnoreCase) && requireStringId)
                {
                    value = value?.ToString();
                }
                else if (value is IDataItem child)
                {
                    value = BuildObject(child);
                }
                else if (value is IEnumerable<object?> array)
                {
                    value = array.Select(dataItem =>
                    {
                        if (dataItem is IDataItem childObject)
                        {
                            return BuildObject(childObject);
                        }
                        return dataItem;
                    }).ToArray();
                }

                item.TryAdd(field, value);
            }

            return item;
        }
    }
}