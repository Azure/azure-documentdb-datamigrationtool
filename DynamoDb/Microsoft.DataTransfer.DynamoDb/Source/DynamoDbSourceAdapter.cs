using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.DynamoDb.Client.Query;
using Microsoft.DataTransfer.DynamoDb.Requests;
using Microsoft.DataTransfer.Extensibility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DynamoDb.Source
{
    sealed class DynamoDbSourceAdapter : IDataSourceAdapter
    {
        private IDataPageProvider dataProvider;

        private Task<DataPage> pageDownloadTask;
        private int pageEntityIndex;
        private int globalEntityIndex;

        public DynamoDbSourceAdapter(IAmazonDynamoDB client, IDynamoDbSourceAdapterInstanceConfiguration configuration)
        {
            Guard.NotNull("client", client);
            Guard.NotNull("configuration", configuration);

            dataProvider = IsQueryRequest(configuration.Request)
                ? (IDataPageProvider)new QueryDataPageProvider(client,
                    RequestReader.Instance.Read<QueryRequest>(configuration.Request))
                : new ScanDataPageProvider(client,
                    RequestReader.Instance.Read<ScanRequest>(configuration.Request));
        }

        private static bool IsQueryRequest(string request)
        {
            using (var reader = new JsonTextReader(new StringReader(request)))
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName && reader.Depth == 1)
                        if ("KeyConditionExpression".Equals(reader.Value as string, StringComparison.OrdinalIgnoreCase) ||
                            "KeyCondition".Equals(reader.Value as string, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                }
            }

            return false;
        }

        public async Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation)
        {
            // TODO: Is there a way to get some sort of an entity identifier?
            readOutput.DataItemId = String.Format(CultureInfo.InvariantCulture, Resources.DataItemIdFormat, ++globalEntityIndex);

            if (pageDownloadTask == null)
            {
                MoveToNextPage(null, cancellation);
            }

            var currentPage = await pageDownloadTask;

            // Make sure current page has data to read
            while (pageEntityIndex >= currentPage.Items.Count && IsValidContinuation(currentPage.ContinuationToken))
            {
                MoveToNextPage(currentPage.ContinuationToken, cancellation);
                currentPage = await pageDownloadTask;
            }

            if (pageEntityIndex >= currentPage.Items.Count && !IsValidContinuation(currentPage.ContinuationToken))
            {
                return null;
            }

            var item = currentPage.Items[pageEntityIndex++];

            if (pageEntityIndex >= currentPage.Items.Count && IsValidContinuation(currentPage.ContinuationToken))
            {
                // Start downloading next page while current item is being processed
                MoveToNextPage(currentPage.ContinuationToken, cancellation);
            }

            return new DynamoDbDataItem(item);
        }

        private bool IsValidContinuation(Dictionary<string, AttributeValue> continuationToken)
        {
            return continuationToken != null && continuationToken.Count != 0;
        }

        private void MoveToNextPage(Dictionary<string, AttributeValue> continuationToken, CancellationToken cancellation)
        {
            pageDownloadTask = dataProvider.LoadPageAsync(continuationToken, cancellation);
            pageEntityIndex = 0;
        }

        public void Dispose()
        {
            TrashCan.Throw(ref dataProvider);
        }
    }
}
