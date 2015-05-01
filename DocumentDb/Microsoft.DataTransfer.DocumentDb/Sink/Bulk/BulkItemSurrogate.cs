using Microsoft.DataTransfer.Extensibility;
using Newtonsoft.Json;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    sealed class BulkItemSurrogate
    {
        [JsonProperty("i")]
        public int DocumentIndex { get; set; }

        [JsonProperty("d")]
        public DataItemSurrogate Document { get; set; }

        public BulkItemSurrogate() { }

        public BulkItemSurrogate(int documentIndex, IDataItem document)
        {
            DocumentIndex = documentIndex;
            Document = new DataItemSurrogate(document);
        }
    }
}
