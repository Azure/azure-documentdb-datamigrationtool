using Amazon.DynamoDBv2.Model;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DynamoDb.Client
{
    /// <summary>
    /// Simple DynamoDB client to verify the connection.
    /// </summary>
    public sealed class DynamoDbProbeClient
    {
        /// <summary>
        /// Tests the DynamoDB connection.
        /// </summary>
        /// <param name="connectionString">DynamoDB connection string to use to connect.</param>
        public async Task TestConnectionAsync(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw Errors.ConnectionStringMissing();

            using (var client = AmazonDynamoDbFactory.Create(connectionString))
            {
                var response = await client.ListTablesAsync(new ListTablesRequest { Limit = 1 });

                if (response.HttpStatusCode != HttpStatusCode.OK)
                    throw Errors.FailedToListTables(Enum.GetName(typeof(HttpStatusCode), response.HttpStatusCode));
            }
        }
    }
}
