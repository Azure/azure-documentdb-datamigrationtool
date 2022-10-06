using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Reflection;
using System.Resources;
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
        public string DisplayName => "Cosmos-nosql";

        public async Task WriteAsync(IAsyncEnumerable<IDataItem> dataItems, IConfiguration config, CancellationToken cancellationToken = default)
        {
            var settings = config.Get<CosmosSinkSettings>();
            settings.Validate();

            // based on:
            //UserAgentSuffix = String.Format(CultureInfo.InvariantCulture, Resources.CustomUserAgentSuffixFormat,
            //    entryAssembly == null ? Resources.UnknownEntryAssembly : entryAssembly.GetName().Name,
            //    Assembly.GetExecutingAssembly().GetName().Version,
            //    context.SourceName, context.SinkName,
            //    isShardedImport ? Resources.ShardedImportDesignator : String.Empty)

            var entryAssembly = Assembly.GetEntryAssembly();
            bool isShardedImport = false;
            string sourceName = "Unknown"; // TODO: add source as parameter
            string sinkName = DisplayName;
            string userAgentString = string.Format(CultureInfo.InvariantCulture, "{0}-{1}-{2}-{3}{4}",
                                    entryAssembly == null ? "dtr" : entryAssembly.GetName().Name,
                                    Assembly.GetExecutingAssembly().GetName().Version,
                                    sourceName, sinkName,
                                    isShardedImport ? "-Sharded" : string.Empty);

            var client = new CosmosClient(settings.ConnectionString,
                new CosmosClientOptions
                {
                    ConnectionMode = settings.ConnectionMode,
                    ApplicationName = userAgentString,
                    AllowBulkExecution = true,
                    EnableContentResponseOnWrite = false,
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

            var containerProperties = new ContainerProperties
            {
                Id = settings.Container,
                PartitionKeyDefinitionVersion = PartitionKeyDefinitionVersion.V2,
                PartitionKeyPath = settings.PartitionKeyPath,
            };

            ThroughputProperties throughputProperties = settings.UseAutoscaleForCreatedContainer
                ? ThroughputProperties.CreateAutoscaleThroughput(settings.CreatedContainerMaxThroughput ?? 4000)
                : ThroughputProperties.CreateManualThroughput(settings.CreatedContainerMaxThroughput ?? 400);

            Container? container = await database.CreateContainerIfNotExistsAsync(containerProperties, throughputProperties, cancellationToken: cancellationToken);

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
            var retry = GetRetryPolicy(settings.MaxRetryCount, settings.InitialRetryDurationMs);
            await foreach (var batch in batches.WithCancellation(cancellationToken))
            {
                var insertTasks = batch.Select(item => InsertItemAsync(container, item, retry, cancellationToken)).ToList();

                var results = await Task.WhenAll(insertTasks);
                ReportCount(results.Sum());
            }

            Console.WriteLine($"Added {insertCount} total records in {timer.ElapsedMilliseconds / 1000.0:F2}s");
        }

        private static AsyncRetryPolicy GetRetryPolicy(int maxRetryCount, int initialRetryDuration)
        {
            int retryDelayBaseMs = initialRetryDuration / 2;
            var jitter = new Random();
            var retryPolicy = Policy
                .Handle<CosmosException>(c => c.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(maxRetryCount,
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