using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DynamoDb.Client.Query
{
    interface IDataPageProvider : IDisposable
    {
        Task<DataPage> LoadPageAsync(Dictionary<string, AttributeValue> continuationToken, CancellationToken cancellation);
    }
}
