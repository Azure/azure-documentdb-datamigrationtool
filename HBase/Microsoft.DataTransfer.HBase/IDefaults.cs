
namespace Microsoft.DataTransfer.HBase
{
    /// <summary>
    /// Contains default configuration for HBase data adapters.
    /// </summary>
    public interface IDefaults
    {
        /// <summary>
        /// Gets the default source batch size.
        /// </summary>
        int SourceBatchSize { get; }
    }
}
