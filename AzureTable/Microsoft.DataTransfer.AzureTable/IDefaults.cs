using Microsoft.DataTransfer.AzureTable.Source;

namespace Microsoft.DataTransfer.AzureTable
{
    /// <summary>
    /// Contains default configuration for Azure Table storage data adapters.
    /// </summary>
    public interface IDefaults
    {
        /// <summary>
        /// Gets the default value that indicates which internal Azure Table fields should be preserved in the source adapter output.
        /// </summary>
        AzureTableInternalFields SourceInternalFields { get; }
    }
}
