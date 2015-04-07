using Newtonsoft.Json;
using System.Collections;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    [JsonConverter(typeof(LengthCappedEnumerableJsonConverter))]
    sealed class LengthCappedEnumerableSurrogate : IEnumerable
    {
        private IEnumerable enumerable;

        public int MaxSerializedLength { get; private set; }

        public LengthCappedEnumerableSurrogate(IEnumerable enumerable, int maxSerializedLength)
        {
            this.enumerable = enumerable;
            MaxSerializedLength = maxSerializedLength;
        }

        public IEnumerator GetEnumerator()
        {
            return enumerable.GetEnumerator();
        }
    }
}
