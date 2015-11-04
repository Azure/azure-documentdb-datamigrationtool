
namespace Microsoft.DataTransfer.ServiceModel.Errors
{
    /// <summary>
    /// Data transfer error details enumeration.
    /// </summary>
    public enum ErrorDetails
    {
        /// <summary>
        /// No detailed error information.
        /// </summary>
        None,

        /// <summary>
        /// Detailed error information for critical errors only.
        /// </summary>
        Critical,

        /// <summary>
        /// Detailed error information for all errors.
        /// </summary>
        All
    }
}
