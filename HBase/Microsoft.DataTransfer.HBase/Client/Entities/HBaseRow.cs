using Microsoft.DataTransfer.HBase.Client.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.HBase.Client.Entities
{
    sealed class HBaseRow
    {
        [JsonProperty("key"), JsonConverter(typeof(Base64StringJsonConverter))]
        public string Key { get; set; }

        [JsonProperty("Cell")]
        public List<HBaseCell> Cells { get; set; }
    }
}
