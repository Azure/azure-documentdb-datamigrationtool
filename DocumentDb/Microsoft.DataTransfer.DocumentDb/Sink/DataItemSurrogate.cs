using Microsoft.DataTransfer.Extensibility;
using Newtonsoft.Json;

namespace Microsoft.DataTransfer.DocumentDb.Sink
{
    [JsonConverter(typeof(DataItemSurrogateJsonConverter))]
    sealed class DataItemSurrogate 
    {
        public IDataItem DataItem { get; private set; }

        public DataItemSurrogate(IDataItem dataItem)
        {
            DataItem = dataItem;
        }
    }
}
