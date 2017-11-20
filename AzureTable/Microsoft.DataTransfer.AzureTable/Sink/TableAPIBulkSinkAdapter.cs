namespace Microsoft.DataTransfer.TableAPI.Sink.Bulk
{
    using Microsoft.Azure.CosmosDB;
    using Microsoft.Azure.CosmosDB.Table;
    using Microsoft.Azure.Storage;
    using Microsoft.DataTransfer.AzureTable.Sink.Bulk;
    using Microsoft.DataTransfer.AzureTable.Source;
    using Microsoft.DataTransfer.AzureTable.Utils;
    using Microsoft.DataTransfer.Basics;
    using Microsoft.DataTransfer.Extensibility;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class TableAPIBulkSinkAdapter : IDataSinkAdapter
    {
        private const long maxLengthInBytesPerDocument = 2 * 1024 * 1024;

        private string _connectionString;
        private string _tableName;
        private bool _overwrite;
        private long _maxInputBufferSizeInBytes;
        private int _throughput;
        private int _maxLengthInBytesPerBatch;

        private CloudTable cloudtable;
        private ConcurrentDictionary<string, TableBatchOperation> dict;
        private InputSizeTracker inputSizeTracker;
        private BatchSizeTracker batchSizeTracker;

        public int MaxDegreeOfParallelism
        {
            get { return 1; }
        }

        public TableAPIBulkSinkAdapter(string connectionString, string tableName, 
            bool overwrite, long maxInputBufferSizeInBytes, int throughput, int batchSize)
        {
            _connectionString = connectionString;
            _tableName = tableName;
            _overwrite = overwrite;
            _maxInputBufferSizeInBytes = maxInputBufferSizeInBytes;
            _throughput = throughput;
            _maxLengthInBytesPerBatch = batchSize;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionString);

            TableConnectionPolicy connectionPolicy = new TableConnectionPolicy()
            {
                UseDirectMode = true,
                UseTcpProtocol = true,
            };

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(connectionPolicy: connectionPolicy);
            cloudtable = tableClient.GetTableReference(_tableName);
        }

        public async Task InitializeAsync(CancellationToken cancellation)
        {
            inputSizeTracker = new InputSizeTracker(maxLengthInBytesPerDocument, _maxInputBufferSizeInBytes);
            batchSizeTracker = new BatchSizeTracker(_maxLengthInBytesPerBatch, inputSizeTracker);
            dict = new ConcurrentDictionary<string, TableBatchOperation>();
            await cloudtable.CreateIfNotExistsAsync(IndexingMode.Consistent, _throughput, cancellation);
        }

        public async Task WriteAsync(IDataItem dataItem, CancellationToken cancellation)
        {
            var item = GetITableEntityFromIDataItem(dataItem);

            inputSizeTracker.Add(item);

            if (!dict.ContainsKey(item.PartitionKey))
            {
                dict[item.PartitionKey] = new TableBatchOperation();
            }

            var batchOperation = dict[item.PartitionKey];
            if (_overwrite)
            {
                batchOperation.InsertOrReplace(item); 
            }
            else
            {
                batchOperation.Insert(item);
            }

            if (inputSizeTracker.HasExceededInputLimits())
            {
                await FlushToTableAsync(cancellation);
            }
        }

        public async Task CompleteAsync(CancellationToken cancellation)
        {
            if (!inputSizeTracker.HasExceededInputLimits())
            {
                await FlushToTableAsync(cancellation);
            }
        }

        public void Dispose()
        {
        }

        // Flush out all data to the Cosmos DB Table sink.
        private async Task FlushToTableAsync(CancellationToken cancellation)
        {
            try
            {
                List<Exception> exceptions = new List<Exception>();

                foreach (var kv in dict)
                {
                    var batchOperation = kv.Value;
                    if (batchOperation.Count > 0)
                    {
                        var subOperations = batchSizeTracker.Split(batchOperation);

                        foreach (var subOperation in subOperations)
                        {
                            var op = new TableBatchOperation();
                            for (int i = 0; i < subOperation.Count(); i++)
                            {
                                op.Add(subOperation.ElementAt(i));
                            }

                            try
                            {
                                await Utils.ExecuteWithRetryAsync(
                                                    () => cloudtable.ExecuteBatchAsync(op, cancellation)
                                                );
                            }
                            catch (Exception ex)
                            {
                                string listofDocumentsNotCommitted = string.Join(",", op.Select(x => x.Entity.RowKey));
                                ex =  new Exception(
                                        string.Format("{0} : offending documents having PartitionKey={1}: RowKeys:[{2}]",
                                        ex.Message, op[0].Entity.PartitionKey, listofDocumentsNotCommitted), ex
                                );
                                exceptions.Add(ex);
                            }
                        }
                    }
                }

                if (exceptions.Count > 0)
                {
                    throw new AggregateException(exceptions);
                }
            }
            finally
            {
                dict.Clear();
                batchSizeTracker.Clear();
            }
        }

        private ITableEntity GetITableEntityFromIDataItem(IDataItem dataItem)
        {
            /* NOTE: Assume that the source is Azure Table.
             * Other sources are NOT supported at this point.
             */

            DynamicTableEntityDataItem tableEntityDataItem = dataItem as DynamicTableEntityDataItem;
            Guard.NotNull("tableEntityDataItem", tableEntityDataItem);

            var sourceData = tableEntityDataItem.GetDynamicTableEntity();

            sourceData.Properties.Remove("RowKey");
            sourceData.Properties.Remove("PartitionKey");
            sourceData.Properties.Remove("Timestamp");

            return sourceData;
        }
    }
}
