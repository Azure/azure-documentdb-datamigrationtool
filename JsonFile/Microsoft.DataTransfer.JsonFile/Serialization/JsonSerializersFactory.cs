using Microsoft.DataTransfer.JsonNet.Serialization;
using Newtonsoft.Json;

namespace Microsoft.DataTransfer.JsonFile.Serialization
{
    static class JsonSerializersFactory
    {
        public static JsonSerializer Create(bool prettify)
        {
            return JsonSerializer.CreateDefault(new JsonSerializerSettings
            {
                Converters =
                {
                    DataItemJsonConverter.Instance,
                    GeoJsonConverter.Instance
                },
                Formatting = prettify ? Formatting.Indented : Formatting.None
            });
        }
    }
}
