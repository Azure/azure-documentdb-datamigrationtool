using Newtonsoft.Json;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    sealed class BulkInsertItemState
    {
        [JsonProperty("i")]
        public int DocumentIndex { get; set; }

        [JsonProperty("e")]
        public string ErrorMessage { get; set; }
    }
}
