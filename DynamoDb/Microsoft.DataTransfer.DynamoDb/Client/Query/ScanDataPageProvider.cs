using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DynamoDb.Client.Query
{
    sealed class ScanDataPageProvider : DataPageProviderBase<ScanRequest>
    {
        public ScanDataPageProvider(IAmazonDynamoDB client, ScanRequest request)
            : base(client, request) { }

        protected override void SetRequestContinuation(ScanRequest request, Dictionary<string, AttributeValue> continuationToken)
        {
            request.ExclusiveStartKey = continuationToken;
        }

        protected override async Task<DataPage> LoadPageAsync(IAmazonDynamoDB client, ScanRequest request, CancellationToken cancellation)
        {
            var response = await client.ScanAsync(request, cancellation);
            return new DataPage(response.Items, response.LastEvaluatedKey);
        }
    }
}
