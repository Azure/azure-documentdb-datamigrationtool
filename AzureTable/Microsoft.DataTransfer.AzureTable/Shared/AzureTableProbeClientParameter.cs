using Microsoft.DataTransfer.AzureTable.Source;

namespace Microsoft.DataTransfer.AzureTable.Wpf.Shared
{
    /// <summary>
    /// A class that encapsulates the Azure Table storage connection string and Location Mode to use to connect to the account.
    /// </summary>
    public class AzureTableProbeClientParameter
    {
        /// <summary>
        /// Connection string for the Azure Table storage.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Location Mode string for the Azure Table storage. 
        /// </summary>
        public AzureTableLocationMode? LocationMode { get; set; }
    }
}
