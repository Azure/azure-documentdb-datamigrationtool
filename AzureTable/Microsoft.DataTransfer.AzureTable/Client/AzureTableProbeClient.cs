using Microsoft.WindowsAzure.Storage;
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
        /// <returns>Task that represents asynchronous connection operation.</returns>
        public async Task TestConnection(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw Errors.ConnectionStringMissing();

            var properties = await CloudStorageAccount.Parse(connectionString).CreateCloudTableClient().GetServicePropertiesAsync();
            if (properties == null)
                throw Errors.EmptyResponseReceived();
        }
    }
}
