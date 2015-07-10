using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DynamoDb.Client.Query
{
    sealed class QueryDataPageProvider : DataPageProviderBase<QueryRequest>
    {
        public QueryDataPageProvider(IAmazonDynamoDB client, QueryRequest request)
            : base(client, request) { }

        protected override void SetRequestContinuation(QueryRequest request, Dictionary<string, AttributeValue> continuationToken)
        {
            request.ExclusiveStartKey = continuationToken;
        }

        protected override async Task<DataPage> LoadPageAsync(IAmazonDynamoDB client, QueryRequest request, CancellationToken cancellation)
        {
            var response = await client.QueryAsync(request, cancellation);
            return new DataPage(response.Items, response.LastEvaluatedKey);
        }
    }
}
