using System.ComponentModel.Composition;
using System.Configuration;
using System.Dynamic;
using Microsoft.Azure.Cosmos;
using Microsoft.DataTransfer.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Microsoft.DataTransfer.CosmosExtension
{
    [Export(typeof(IDataTransferExtension))]
    public class CosmosDataTransferExtension : IDataTransferExtension
    {
        private string? _connectionString;
        public string DisplayName => "Cosmos DB";
        public IAsyncEnumerable<IDataItem> ReadAsSourceAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task WriteAsSinkAsync(IAsyncEnumerable<IDataItem> dataItems, CancellationToken cancellationToken = default)
        {
            var client = new CosmosClient(_connectionString,
                new CosmosClientOptions
                {
                    ConnectionMode = ConnectionMode.Gateway,
                    AllowBulkExecution = true
                });

            var database = client.GetDatabase("myDb");
            try
            {
                await database.GetContainer("myContainer").DeleteContainerAsync(cancellationToken: cancellationToken);
            }
            catch { }
            Container? container = await database.CreateContainerIfNotExistsAsync("myContainer", "/id", cancellationToken: cancellationToken);

            var createTasks = new List<Task>();
            await foreach (var source in dataItems.WithCancellation(cancellationToken))
            {
                var task = InsertItem(container, source, cancellationToken);
                createTasks.Add(task);
            }

            await Task.WhenAll(createTasks);
        }

        public Task Configure(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("TargetConnectionString");
            return Task.CompletedTask;
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
                else if (value is IEnumerable<IDataItem> array)
                {
                    value = array.Select(dataItem => BuildObject(dataItem)).ToArray();
                }

                item.TryAdd(field, value);
            }

            return item;
        }
    }
}