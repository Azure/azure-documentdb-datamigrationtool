
namespace Microsoft.DataTransfer.AzureTable.Source
{
    /// <summary>
    /// Azure Table storage internal fields enumeration.
    /// </summary>
        
    public enum AzureTableLocationMode
    {
        /// <summary>
        /// Primary Only
        /// </summary>
        PrimaryOnly,

        /// <summary>
        /// Primary then Secondary
        /// </summary>
        PrimaryThenSecondary,

        /// <summary>
        /// Secondary Only
        /// </summary>
        SecondaryOnly,

        /// <summary>
        /// Secondary then Primary
        /// </summary>
        SecondaryThenPrimary
    }
}
