using Microsoft.DataTransfer.Extensibility;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.AzureTable.Source
{
    sealed class AzureTableSourceAdapter : IDataSourceAdapter
    {
        private const string RowKeyFieldName = "RowKey";
        private const string PartitionKeyFieldName = "PartitionKey";
        private const string TimestampFieldName = "Timestamp";

        private readonly IAzureTableSourceAdapterInstanceConfiguration configuration;
        private readonly CloudTable table;
        private readonly TableQuery query;
        private readonly TableRequestOptions requestOptions;

        private Task<TableQuerySegment<DynamicTableEntity>> segmentDownloadTask;
        private int currentEntityIndex;

        public AzureTableSourceAdapter(IAzureTableSourceAdapterInstanceConfiguration configuration)
        {
            this.configuration = configuration;

            table = CloudStorageAccount.Parse(configuration.ConnectionString).CreateCloudTableClient().GetTableReference(configuration.Table);

            /// If the location mode passed is null (the user has left it at default settings, do not bother changing LocationMode on the client.
            if (configuration.LocationMode != null)
            {
                requestOptions = new TableRequestOptions
                    {
                        LocationMode = AzureTableSourceAdapter.ToAzureLocationMode(configuration.LocationMode)
                    };
            }
          
            query = new TableQuery
            {
                FilterString = configuration.Filter,
                SelectColumns = configuration.Projection == null ? null : new List<string>(configuration.Projection)  
            };
        }

        public static LocationMode? ToAzureLocationMode(AzureTableLocationMode? mode)
        {
            switch (mode)
            {
                case AzureTableLocationMode.PrimaryOnly:
                    return LocationMode.PrimaryOnly;
                    
                case AzureTableLocationMode.PrimaryThenSecondary:
                    return LocationMode.PrimaryThenSecondary;                    

                case AzureTableLocationMode.SecondaryOnly:
                    return LocationMode.SecondaryOnly;                    

                case AzureTableLocationMode.SecondaryThenPrimary:
                    return LocationMode.SecondaryThenPrimary;
                    
                default:
                    return null;
            }      
        }

        public async Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation)
        {
            if (segmentDownloadTask == null)
            {
                MoveToNextSegment(null, cancellation);
            }

            var currentSegment = await segmentDownloadTask;

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

        private DynamicTableEntity AppendInternalProperties(DynamicTableEntity entity)
        {
            if (configuration.InternalFields == AzureTableInternalFields.None)
                return entity;

            if (configuration.InternalFields == AzureTableInternalFields.All)
            {
                entity.Properties[PartitionKeyFieldName] = new EntityProperty(entity.PartitionKey);
                entity.Properties[TimestampFieldName] = new EntityProperty(entity.Timestamp);
            }

            entity.Properties[RowKeyFieldName] = new EntityProperty(entity.RowKey);

            return entity;
        }

        private void MoveToNextSegment(TableContinuationToken continuationToken, CancellationToken cancellation)
        {
            segmentDownloadTask = table.ExecuteQuerySegmentedAsync(query, continuationToken, requestOptions, null, cancellation);
            currentEntityIndex = 0;
        }

        public void Dispose() { }
    }
}
