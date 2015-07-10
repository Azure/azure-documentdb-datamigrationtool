using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.DataTransfer.Basics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DynamoDb.Client.Query
{
    abstract class DataPageProviderBase<TRequest> : IDataPageProvider
    {
        private IAmazonDynamoDB client;
        private TRequest request;

        public DataPageProviderBase(IAmazonDynamoDB client, TRequest request)
        {
            this.client = client;
            this.request = request;
        }

        public Task<DataPage> LoadPageAsync(Dictionary<string, AttributeValue> continuationToken, CancellationToken cancellation)
        {
            SetRequestContinuation(request, continuationToken);
            return LoadPageAsync(client, request, cancellation);
        }

        protected abstract void SetRequestContinuation(TRequest request, Dictionary<string, AttributeValue> continuationToken);

        protected abstract Task<DataPage> LoadPageAsync(IAmazonDynamoDB client, TRequest request, CancellationToken cancellation);

        public void Dispose()
        {
            TrashCan.Throw(ref client);
        }
    }
}
