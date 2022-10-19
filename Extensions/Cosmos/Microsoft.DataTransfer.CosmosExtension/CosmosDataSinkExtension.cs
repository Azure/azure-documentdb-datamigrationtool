using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Dynamic;
using Microsoft.Azure.Cosmos;
using Microsoft.DataTransfer.Interfaces;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Retry;

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

            int insertCount = 0;

            var timer = Stopwatch.StartNew();
            void ReportCount(int i)
            {
                insertCount += i;
                if (insertCount % 500 == 0)
                {
                    Console.WriteLine($"{insertCount} records added after {timer.ElapsedMilliseconds / 1000.0:F2}s");
                }
            }

            var convertedObjects = dataItems.Select(di => BuildObject(di, true)).Where(o => o != null).OfType<ExpandoObject>();
            var batches = convertedObjects.Buffer(settings.BatchSize);
            var retry = GetRetryPolicy();
            await foreach (var batch in batches.WithCancellation(cancellationToken))
            {
                var tasks = settings.UpdateExistingDocuments
                    ? batch.Select(item => UpsertItemAsync(container, item, retry, cancellationToken)).ToList()
                    : batch.Select(item => InsertItemAsync(container, item, retry, cancellationToken)).ToList();

                var results = await Task.WhenAll(tasks);
                ReportCount(results.Sum());
            }

            Console.WriteLine($"Added {insertCount} total records in {timer.ElapsedMilliseconds / 1000.0:F2}s");
        }

        private static AsyncRetryPolicy GetRetryPolicy()
        {
            const int retryCount = 5;
            const int retryDelayBaseMs = 100;

            var jitter = new Random();
            var retryPolicy = Policy
                .Handle<CosmosException>(c => c.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(retryCount,
                    retryAttempt => TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempt) * retryDelayBaseMs + jitter.Next(0, retryDelayBaseMs))
                );

            return retryPolicy;
        }

        private static Task<int> InsertItemAsync(Container container, ExpandoObject item, AsyncRetryPolicy retryPolicy, CancellationToken cancellationToken)
        {
            var task = retryPolicy.ExecuteAsync(() => container.CreateItemAsync(item, cancellationToken: cancellationToken))
                .ContinueWith(t =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        return 1;
                    }

                    if (t.IsFaulted)
                    {
                        Console.WriteLine($"Error adding record: {t.Exception?.Message}");
                    }

                    return 0;
                }, cancellationToken);
            return task;
        }

        private static Task<int> UpsertItemAsync(Container container, ExpandoObject item, AsyncRetryPolicy retryPolicy, CancellationToken cancellationToken)
        {
            var task = retryPolicy.ExecuteAsync(() => container.UpsertItemAsync(item, cancellationToken: cancellationToken))
                .ContinueWith(t =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        return 1;
                    }

                    if (t.IsFaulted)
                    {
                        Console.WriteLine($"Error adding/replacing record: {t.Exception?.Message}");
                    }

                    return 0;
                }, cancellationToken);
            return task;
        }

        private static ExpandoObject? BuildObject(IDataItem? source, bool requireStringId = false)
        {
            if (source == null)
                return null;

            var fields = source.GetFieldNames().ToList();
            var item = new ExpandoObject();
            if (requireStringId && !fields.Contains("id", StringComparer.CurrentCultureIgnoreCase))
            {
                item.TryAdd("id", Guid.NewGuid().ToString());
            }
            foreach (string field in fields)
            {
                object? value = source.GetValue(field);
                var fieldName = field;
                if (string.Equals(field, "id", StringComparison.CurrentCultureIgnoreCase) && requireStringId)
                {
                    value = value?.ToString();
                    fieldName = "id";
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

                item.TryAdd(fieldName, value);
            }

            return item;
        }
    }
}