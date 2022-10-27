using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using Microsoft.Azure.Cosmos;
using Microsoft.DataTransfer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;

namespace Microsoft.DataTransfer.CosmosExtension
{
    [Export(typeof(IDataSinkExtension))]
    public class CosmosDataSinkExtension : IDataSinkExtension
    {
        public string DisplayName => "Cosmos-nosql";

        public async Task WriteAsync(IAsyncEnumerable<IDataItem> dataItems, IConfiguration config, IDataSourceExtension dataSource, ILogger logger, CancellationToken cancellationToken = default)
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
            string sourceName = dataSource.DisplayName;
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
                    logger.LogInformation("{InsertCount} records added after {TotalSeconds}s", insertCount, $"{timer.ElapsedMilliseconds / 1000.0:F2}");
                }
            }

            var convertedObjects = dataItems.Select(di => BuildObject(di, true)).Where(o => o != null).OfType<ExpandoObject>();
            var batches = convertedObjects.Buffer(settings.BatchSize);
            var retry = GetRetryPolicy(settings.MaxRetryCount, settings.InitialRetryDurationMs);
            await foreach (var batch in batches.WithCancellation(cancellationToken))
            {
                var insertTasks = settings.InsertStreams
                    ? batch.Select(item => InsertItemStreamAsync(container, item, settings.PartitionKeyPath, retry, logger, cancellationToken)).ToList()
                    : batch.Select(item => InsertItemAsync(container, item, retry, logger, cancellationToken)).ToList();

                var results = await Task.WhenAll(insertTasks);
                ReportCount(results.Sum());
            }

            logger.LogInformation("Added {InsertCount} total records in {TotalSeconds}s", insertCount, $"{timer.ElapsedMilliseconds / 1000.0:F2}");
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

        private static Task<int> InsertItemAsync(Container container, ExpandoObject item, AsyncRetryPolicy retryPolicy, ILogger logger, CancellationToken cancellationToken)
        {
            logger.LogTrace("Inserting item {Id}", GetPropertyValue(item, "id"));
            var task = retryPolicy.ExecuteAsync(() => container.CreateItemAsync(item, cancellationToken: cancellationToken))
                .ContinueWith(t =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        return 1;
                    }

                    if (t.IsFaulted)
                    {
                        logger.LogWarning(t.Exception, "Error adding record: {ErrorMessage}", t.Exception?.Message);
                    }

                    return 0;
                }, cancellationToken);
            return task;
        }

        private static Task<int> InsertItemStreamAsync(Container container, ExpandoObject item, string? partitionKeyPath, AsyncRetryPolicy retryPolicy, ILogger logger, CancellationToken cancellationToken)
        {
            if (partitionKeyPath == null)
                throw new ArgumentNullException(nameof(partitionKeyPath));

            logger.LogTrace("Inserting item {Id}", GetPropertyValue(item, "id"));
            var json = JsonConvert.SerializeObject(item);

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var task = retryPolicy.ExecuteAsync(() => container.CreateItemStreamAsync(ms, new PartitionKey(GetPropertyValue(item, partitionKeyPath.TrimStart('/'))), cancellationToken: cancellationToken))
                .ContinueWith(t =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        return 1;
                    }

                    if (t.IsFaulted)
                    {
                        logger.LogWarning(t.Exception, "Error adding record: {ErrorMessage}", t.Exception?.Message);
                    }

                    return 0;
                }, cancellationToken);
            return task;
        }

        private static string? GetPropertyValue(ExpandoObject item, string propertyName)
        {
            return ((IDictionary<string, object?>)item)[propertyName]?.ToString();
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