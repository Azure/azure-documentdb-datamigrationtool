using Microsoft.Azure.Documents.Client;
using Microsoft.DataTransfer.DocumentDb.Shared;
using System;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Client
{
    /// <summary>
    /// Simple DocumentDB client to verify the connection.
    /// </summary>
    public sealed class DocumentDbProbeClient
    {
        /// <summary>
        /// Tests the DocumentDB connection.
        /// </summary>
        /// <param name="connectionString">DocumentDB connection string to use to connect to the account.</param>
        /// <param name="connectionMode">DocumentDB connectio mode to use when testing the connection.</param>
        /// <returns>Task that represents asynchronous connection operation.</returns>
        public async Task TestConnection(string connectionString, DocumentDbConnectionMode? connectionMode)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw Errors.ConnectionStringMissing();

            var parsed = DocumentDbConnectionStringBuilder.Parse(connectionString);

            if (String.IsNullOrEmpty(parsed.AccountEndpoint))
                throw Errors.AccountEndpointMissing();

            if (String.IsNullOrEmpty(parsed.AccountKey))
                throw Errors.AccountKeyMissing();

            if (String.IsNullOrEmpty(parsed.Database))
                throw Errors.DatabaseNameMissing();

            var connectionPolicy =  DocumentDbClientHelper.ApplyConnectionMode(new ConnectionPolicy(), connectionMode);

            using (var client = new DocumentClient(new Uri(parsed.AccountEndpoint), parsed.AccountKey, connectionPolicy))
            {
                await client.OpenAsync();
            }
        }
    }
}
