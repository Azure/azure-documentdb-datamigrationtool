
namespace Microsoft.DataTransfer.DocumentDb.Shared
{
    /// <summary>
    /// DocumentDB connection modes enumeration.
    /// </summary>
    public enum DocumentDbConnectionMode
    {
        /// <summary>
        /// Direct connection using binary TCP protocol.
        /// </summary>
        DirectTcp,

        /// <summary>
        /// Direct connection using HTTPS protocol.
        /// </summary>
        DirectHttps,

        /// <summary>
        /// Connect through the gateway.
        /// </summary>
        Gateway
    }
}
