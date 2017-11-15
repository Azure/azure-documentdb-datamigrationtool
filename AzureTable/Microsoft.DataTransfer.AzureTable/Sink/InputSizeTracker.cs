using Microsoft.Azure.CosmosDB.Table;
using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.TableAPI.Sink.Bulk
{
    /// <summary>
    /// Keeps track of the bytes accumulated in memory from input
    /// </summary>
    public class InputSizeTracker
    {
        private long totalInputSizeInBytes;
        private readonly long _maxLengthPerDocument;
        private readonly long _maxBytesToAccumulateInMemory;
        private Dictionary<string, long> sizeMap;

        /// <summary>
        /// Create a new InputSizeTracker
        /// </summary>
        /// <param name="maxLengthPerDocument">Maximum length in bytes allowed, per document</param>
        /// <param name="maxBytesToAccumulateInMemory">
        /// Maximum length in bytes allowed to be accumulated in memory before flushing to sink
        /// </param>
        public InputSizeTracker(long maxLengthPerDocument, long maxBytesToAccumulateInMemory)
        {
            _maxLengthPerDocument = maxLengthPerDocument;
            _maxBytesToAccumulateInMemory = maxBytesToAccumulateInMemory;
            sizeMap = new Dictionary<string, long>();
        }

        /// <summary>
        /// Add an item to the input tracker.
        /// </summary>
        /// <param name="entity">The document being tracked</param>
        public void Add(ITableEntity entity)
        {
            long serializedObjectLength = findLength(entity);

            if (serializedObjectLength > _maxLengthPerDocument)
            {
                throw new NotSupportedException(
                    string.Format("Max document length supported is {0} bytes, current document is {1} bytes, PartitionKey: {2}, RowKey: {3}",
                    _maxLengthPerDocument, serializedObjectLength,
                    entity.PartitionKey, entity.RowKey)
                );
            }

            sizeMap.Add(DocumentKey(entity.PartitionKey, entity.RowKey), serializedObjectLength);
            totalInputSizeInBytes += serializedObjectLength;
        }

        private string DocumentKey(string partitionKey, string rowKey)
        {
            return partitionKey + "," + rowKey;
        }

        private long findLength(object item)
        {
            DynamicTableEntity entity = item as DynamicTableEntity;
            long length = 0;
            length += entity.PartitionKey.Length;
            length += entity.RowKey.Length;

            foreach (var field in entity.Properties)
            {
                length += field.Key.Length;

                switch (field.Value.PropertyType)
                {
                    case EdmType.String:
                        length += field.Value.StringValue.Length;
                        break;
                    case EdmType.Binary:
                        length += field.Value.BinaryValue.Length;
                        break;
                    case EdmType.Boolean:
                        length += sizeof(bool);
                        break;
                    case EdmType.DateTime:
                        length += sizeof(long);
                        break;
                    case EdmType.Double:
                        length += sizeof(double);
                        break;
                    case EdmType.Guid:
                        length += 16;
                        break;
                    case EdmType.Int32:
                        length += sizeof(Int32);
                        break;
                    case EdmType.Int64:
                        length += sizeof(Int64);
                        break;
                    default:
                        break;
                }
            }

            return length;
        }

        /// <summary>
        /// Reset the input tracker
        /// </summary>
        public void Clear()
        {
            totalInputSizeInBytes = 0;
            sizeMap.Clear();
        }

        /// <summary>
        /// Has the input side limit been exceeded
        /// </summary>
        /// <returns>Boolean indicating that the input memory limit has been exceeded</returns>
        public bool HasExceededInputLimits()
        {
            return totalInputSizeInBytes >= _maxBytesToAccumulateInMemory;
        }

        /// <summary>
        /// Get the length of the document specified by the partitionkey and rowkey
        /// </summary>
        /// <param name="partitionKey">The document's partitionkey</param>
        /// <param name="rowKey">The document's rowkey</param>
        /// <returns></returns>
        public long GetDocumentLength(string partitionKey, string rowKey)
        {
            return sizeMap[DocumentKey(partitionKey, rowKey)];
        }
    }
}
