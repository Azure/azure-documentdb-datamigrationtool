using Microsoft.DataTransfer.Extensibility;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    sealed class BulkItemSurrogate
    {
        public int DocumentIndex { get; set; }

        public DataItemSurrogate Document { get; set; }

        public BulkItemSurrogate() { }

        public BulkItemSurrogate(int documentIndex, IDataItem document)
        {
            DocumentIndex = documentIndex;
            Document = new DataItemSurrogate(document);
        }
    }
}
