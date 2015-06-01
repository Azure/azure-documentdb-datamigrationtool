
namespace Microsoft.DataTransfer.RavenDb
{
    /// <summary>
    /// Contains default configuration for RavenDB data adapters.
    /// </summary>
    public interface IDefaults
    {
        /// <summary>
        /// Gets the default index name used for queries.
        /// </summary>
        string SourceIndex { get; }
    }
}
