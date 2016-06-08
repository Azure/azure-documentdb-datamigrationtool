using Microsoft.DataTransfer.AzureTable.Source;
using Microsoft.DataTransfer.AzureTable.Wpf.Shared;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
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
        public async Task TestConnection(AzureTableProbeClientParameter azureTableProbeClientParameter)
        {
            if (String.IsNullOrEmpty(azureTableProbeClientParameter.ConnectionString))
                throw Errors.ConnectionStringMissing();

            CloudStorageAccount account = CloudStorageAccount.Parse(azureTableProbeClientParameter.ConnectionString);
            CloudTableClient client = account.CreateCloudTableClient();

            var properties = await client.GetServicePropertiesAsync();
            if (properties == null)
            {
                throw Errors.EmptyResponseReceived();
            }

           /// If a secondary specific setting has been requested, make sure that they have a secondary configured. 
           if (azureTableProbeClientParameter.LocationMode != null && azureTableProbeClientParameter.LocationMode != AzureTableLocationMode.PrimaryOnly)
           {
               try
               {
                   client.DefaultRequestOptions.LocationMode = LocationMode.SecondaryOnly;
                   var props = await client.GetServicePropertiesAsync();
               }
               catch (StorageException e)
               {
                   throw Errors.SecondaryNotDefined(e.Message);
               }
           }
        }
    }
}
