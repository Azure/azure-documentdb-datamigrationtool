
namespace Microsoft.DataTransfer.Extensibility
{
    /// <summary>
    /// Provides additional information about the data transfer operation.
    /// </summary>
    public interface IDataTransferContext
    {
        /// <summary>
        /// Gets name of the data source adapter.
        /// </summary>
        string SourceName { get; }

        /// <summary>
        /// Gets name of the data sink adapter.
        /// </summary>
        string SinkName { get; }

        /// <summary>
        /// Gets the signature of the source and sink configuration
        /// </summary>
        string RunConfigSignature { get; }

        /// <summary>
        /// Whether to enable the resume function
        /// </summary>
        bool EnableResumeFunction { get; }
    }
}
