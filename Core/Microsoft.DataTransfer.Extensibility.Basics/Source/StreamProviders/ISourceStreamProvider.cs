using System.IO;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Extensibility.Basics.Source.StreamProviders
{
    /// <summary>
    /// Provides <see cref="StreamReader" /> instance to read data from the source stream.
    /// </summary>
    public interface ISourceStreamProvider
    {
        /// <summary>
        /// Gets the identifier of the stream.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Creates a new <see cref="StreamReader" /> to read data from the stream.
        /// </summary>
        /// <returns>Task that represents asynchronous create operation.</returns>
        Task<StreamReader> CreateReader();
    }
}
