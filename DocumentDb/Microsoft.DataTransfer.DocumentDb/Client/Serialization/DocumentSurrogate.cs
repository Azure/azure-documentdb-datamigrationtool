using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Client.Serialization
{
    [JsonConverter(typeof(DocumentSurrogateJsonConverter))]
    sealed class DocumentSurrogate
    {
        public IReadOnlyDictionary<string, object> Properties { get; private set; }

        public DocumentSurrogate(IReadOnlyDictionary<string, object> properties)
        {
            Properties = properties;
        }
    }
}
