using Microsoft.Azure.CosmosDB.Table;
using Microsoft.DataTransfer.TableAPI.Sink.Bulk;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.AzureTable.Sink.Bulk
{
    /// <summary>
    ///  Used to track the total size of documents included in a batch operation
    /// </summary>
    public class BatchSizeTracker
    {
        private InputSizeTracker _inputSizeTracker;
        private long _maxBatchSizeInBytes;
        private const int _maxEntriesPerBatch = 100;

        /// <summary>
        /// Create an instance of the batch size tracker
        /// </summary>
        /// <param name="maxBatchSizeInBytes">The max size of documents, in bytes, allowed per batch</param>
        /// <param name="inputSizeTracker">The input size tracker used to calculate the size of documents</param>
        public BatchSizeTracker(long maxBatchSizeInBytes, InputSizeTracker inputSizeTracker)
        {
            _maxBatchSizeInBytes = maxBatchSizeInBytes;
            _inputSizeTracker = inputSizeTracker;
        }

        /// <summary>
        /// Splits a list of TableOperations into smaller batches that honor CosmosDB storage limits
        /// </summary>
        /// <param name="list">The input list of TableOperations to split into batches</param>
        /// <returns>A list of TableOperation batches that honor CosmosDB storage limits</returns>
        public IEnumerable<IEnumerable<TableOperation>> Split(IEnumerable<TableOperation> list)
        {
            var result = new List<List<TableOperation>>();
            long sum = 0;
            int numEntries = 0;

            foreach (var op in list)
            {
                var entity = op.Entity;
                long docLength = _inputSizeTracker.GetDocumentLength(entity.PartitionKey, entity.RowKey);

                if (result.Count > 0 && 
                    (sum += docLength) <= _maxBatchSizeInBytes && 
                    (numEntries += 1) <= _maxEntriesPerBatch)
                    result[result.Count - 1].Add(op);
                else
                {
                    sum = docLength;
                    numEntries = 1;
                    result.Add(new List<TableOperation> { op });
                }
            }

            return result;
        }

        /// <summary>
        /// Reset the batch size tracker
        /// </summary>
        public void Clear()
        {
            _inputSizeTracker.Clear();
        }
    }
}
