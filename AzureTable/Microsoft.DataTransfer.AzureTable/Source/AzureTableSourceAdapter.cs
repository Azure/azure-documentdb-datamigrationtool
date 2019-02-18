using Microsoft.Azure.CosmosDB.Table;
using Microsoft.Azure.Storage;
using Microsoft.DataTransfer.AzureTable.RemoteLogging;
using Microsoft.DataTransfer.Extensibility;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.AzureTable.Source
{
    sealed class AzureTableSourceAdapter : IDataSourceAdapter
    {
        private const string RowKeyFieldName = "RowKey";
        private const string PartitionKeyFieldName = "PartitionKey";
        private const string TimestampFieldName = "SourceTimestamp";
        private const string ETagFieldName = "SourceETag";

        private readonly IAzureTableSourceAdapterInstanceConfiguration configuration;
        private readonly CloudTable table;
        private readonly TableQuery query;

        private Task<TableQuerySegment<DynamicTableEntity>> segmentDownloadTask;
        private int currentEntityIndex;

        public AzureTableSourceAdapter(IAzureTableSourceAdapterInstanceConfiguration configuration)
        {
            this.configuration = configuration;

            string connectionString = System.Text.RegularExpressions.Regex.Replace(
                configuration.ConnectionString, @"(TableEndpoint=https://)(.*\.)(documents)(\.azure\.com)",
                m => m.Groups[1].Value + m.Groups[2].Value + "table.cosmosdb" + m.Groups[4].Value);

            TableConnectionPolicy connectionPolicy = new TableConnectionPolicy()
            {
                UseDirectMode = true,
                UseTcpProtocol = true,
            };
            var client = CloudStorageAccount.Parse(connectionString).CreateCloudTableClient(connectionPolicy: connectionPolicy);

            //client.DefaultRequestOptions.LocationMode =
            //    AzureTableClientHelper.ToSdkLocationMode(configuration.LocationMode);

            table = client.GetTableReference(configuration.Table);
            query = new TableQuery
            {
                FilterString = configuration.Filter,
                SelectColumns = configuration.Projection == null ? null : new List<string>(configuration.Projection)
            };
        }

        public async Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation)
        {
            TableQuerySegment<DynamicTableEntity> currentSegment = null;
            RemoteLoggingClientProvider remoteLoggingClientProvider = new RemoteLoggingClientProvider();
            IRemoteLogging remoteLogger = remoteLoggingClientProvider.GetRemoteLogger("tableapibulk");

            try
            {
                if (segmentDownloadTask == null)
                {
                    MoveToNextSegment(null, cancellation);
                }

                currentSegment = await segmentDownloadTask;

                // Make sure current segment has data to read
                while (currentEntityIndex >= currentSegment.Results.Count && currentSegment.ContinuationToken != null)
                {
                    MoveToNextSegment(currentSegment.ContinuationToken, cancellation);
                    currentSegment = await segmentDownloadTask;
                }

                if (currentEntityIndex >= currentSegment.Results.Count && currentSegment.ContinuationToken == null)
                {
                    return null;
                }

                var entity = currentSegment.Results[currentEntityIndex++];
                readOutput.DataItemId = entity.RowKey;

                if (currentEntityIndex >= currentSegment.Results.Count && currentSegment.ContinuationToken != null)
                {
                    // Start downloading next segment while current record is being processed
                    MoveToNextSegment(currentSegment.ContinuationToken, cancellation);
                }

                return new DynamicTableEntityDataItem(AppendInternalProperties(entity));
            }
            catch(Exception excp)
            {
                // only enable remote logging for AzureTable -> Cosmos TableAPI
                if (this.configuration.SinkContext.Equals("TableAPIBulk") && this.configuration.SourceContext.Equals("AzureTable"))
                {
                    if (remoteLogger != null)
                        remoteLogger.LogFailures(currentSegment.Results[currentEntityIndex].PartitionKey,
                            currentSegment.Results[currentEntityIndex].RowKey, excp.ToString());
                }
                throw;
            }
        }

        private DynamicTableEntity AppendInternalProperties(DynamicTableEntity entity)
        {
            if (configuration.InternalFields == AzureTableInternalFields.None)
                return entity;

            if (configuration.InternalFields == AzureTableInternalFields.All)
            {
                entity.Properties[PartitionKeyFieldName] = new EntityProperty(entity.PartitionKey);
                entity.Properties[TimestampFieldName] = new EntityProperty(entity.Timestamp);
                entity.Properties[ETagFieldName] = new EntityProperty(entity.ETag);
            }

            entity.Properties[RowKeyFieldName] = new EntityProperty(entity.RowKey);

            return entity;
        }

        private void MoveToNextSegment(TableContinuationToken continuationToken, CancellationToken cancellation)
        {
            segmentDownloadTask = table.ExecuteQuerySegmentedAsync(query, continuationToken, cancellation);
            currentEntityIndex = 0;
        }

        public void Dispose() { }
    }
}
