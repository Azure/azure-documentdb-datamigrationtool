
namespace Microsoft.DataTransfer.Extensibility
{
    /// <summary>
    /// Provides basic information about data adapter.
    /// </summary>
    public interface IDataAdapterFactory
    {
        /// <summary>
        /// Gets the description of the data adapter.
        /// </summary>
        string Description { get; }
    }
}
