using Microsoft.DataTransfer.HBase.Client.Serialization;
using Newtonsoft.Json;

namespace Microsoft.DataTransfer.HBase.Client.Entities
{
    sealed class HBaseCell
    {
        [JsonProperty("column"), JsonConverter(typeof(Base64StringJsonConverter))]
        public string ColumnName { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("$"), JsonConverter(typeof(Base64StringJsonConverter))]
        public string Value { get; set; }
    }
}
