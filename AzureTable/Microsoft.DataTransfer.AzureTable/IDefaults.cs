using Microsoft.DataTransfer.AzureTable.Shared;
using Microsoft.DataTransfer.AzureTable.Source;

namespace Microsoft.DataTransfer.AzureTable
{
    /// <summary>
    /// Contains default configuration for Azure Table storage data adapters.
    /// </summary>
    public interface IDefaults
    {
        /// <summary>
        /// Gets the default value that indicates which location mode should be used when connecting to Azure Table storage.
        /// </summary>
        AzureStorageLocationMode LocationMode { get; }

        /// <summary>
        /// Gets the default value that indicates which internal Azure Table fields should be preserved in the source adapter output.
        /// </summary>
        AzureTableInternalFields SourceInternalFields { get; }
    }
}
