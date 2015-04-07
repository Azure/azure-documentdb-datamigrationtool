
namespace Microsoft.DataTransfer.DocumentDb.Sink
{
    /// <summary>
    /// DocumentDB date and time handling strategy enumeration.
    /// </summary>
    public enum DateTimeHandling
    {
        /// <summary>
        /// Persist date and time as string.
        /// </summary>
        String,

        /// <summary>
        /// Persist date and time as epoch time.
        /// </summary>
        Epoch,

        /// <summary>
        /// Persist date and time as string and as epoch time.
        /// </summary>
        Both
    }
}
