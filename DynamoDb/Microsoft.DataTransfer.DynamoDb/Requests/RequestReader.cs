using Amazon.DynamoDBv2;
using Newtonsoft.Json;

namespace Microsoft.DataTransfer.DynamoDb.Requests
{
    sealed class RequestReader
    {
        public static readonly RequestReader Instance = new RequestReader();

        public T Read<T>(string request)
            where T : AmazonDynamoDBRequest
        {
            return JsonConvert.DeserializeObject<T>(request, MemoryStreamJsonConverter.Instance);
        }
    }
}
