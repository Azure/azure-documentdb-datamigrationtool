using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DynamoDb.Client.Query
{
    sealed class DataPage
    {
        public IReadOnlyList<IReadOnlyDictionary<string, AttributeValue>> Items { get; private set; }
        public Dictionary<string, AttributeValue> ContinuationToken { get; private set; }

        public DataPage(IReadOnlyList<IReadOnlyDictionary<string, AttributeValue>> items, Dictionary<string, AttributeValue> continuationToken)
        {
            Items = items;
            ContinuationToken = continuationToken;
        }
    }
}
