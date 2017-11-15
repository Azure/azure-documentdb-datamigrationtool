using Microsoft.Azure.CosmosDB.Table;
using Microsoft.Azure.Storage;
using Microsoft.DataTransfer.AzureTable.Shared;
using System;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.AzureTable.Client
{
    /// <summary>
    /// Simple Azure Table storage client to verify the connection.
    /// </summary>
    public sealed class AzureTableProbeClient
    {
        /// <summary>
        /// Tests the Azure Table storage connection.
        /// </summary>
        /// <param name="connectionString">Azure Table storage connection string to use to connect to the account.</param>
        /// <param name="locationMode">Location mode to use when connecting to Azure Table storage.</param>
        /// <returns>Task that represents asynchronous connection operation.</returns>
        public async Task TestConnection(string connectionString, AzureStorageLocationMode? locationMode)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw Errors.ConnectionStringMissing();

            var client = CloudStorageAccount.Parse(connectionString).CreateCloudTableClient();
            client.DefaultRequestOptions.LocationMode = AzureTableClientHelper.ToSdkLocationMode(locationMode);

            var properties = await client.GetServicePropertiesAsync();
            if (properties == null)
                throw Errors.EmptyResponseReceived();
        }
    }
}
