namespace Microsoft.DataTransfer.AzureTable.Shared
{
    /// <summary>
    /// Azure Storage location modes enumeration.
    /// </summary>
    public enum AzureStorageLocationMode
    {
        /// <summary>
        /// Connect to the primary replica only.
        /// </summary>
        PrimaryOnly,

        /// <summary>
        /// Try connect to the primary replica and fallback to secondary.
        /// </summary>
        PrimaryThenSecondary,

        /// <summary>
        /// Connect to the secondary replica only.
        /// </summary>
        SecondaryOnly,

        /// <summary>
        /// Try connect to the secondary replica and fallback to primary.
        /// </summary>
        SecondaryThenPrimary
    }
}
