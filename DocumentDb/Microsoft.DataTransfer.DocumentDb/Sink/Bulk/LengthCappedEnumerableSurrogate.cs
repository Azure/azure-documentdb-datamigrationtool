using Newtonsoft.Json;
using System.Collections;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    [JsonConverter(typeof(LengthCappedEnumerableJsonConverter))]
    sealed class LengthCappedEnumerableSurrogate : IEnumerable
    {
        private IEnumerable enumerable;

        public int MaxElements { get; private set; }
        public int MaxSerializedLength { get; private set; }

        public int LastSerializedCount { get; set; }

        public LengthCappedEnumerableSurrogate(IEnumerable enumerable, int maxElements, int maxSerializedLength)
        {
            this.enumerable = enumerable;
            MaxElements = maxElements;
            MaxSerializedLength = maxSerializedLength;
        }

        public IEnumerator GetEnumerator()
        {
            return enumerable.GetEnumerator();
        }
    }
}
