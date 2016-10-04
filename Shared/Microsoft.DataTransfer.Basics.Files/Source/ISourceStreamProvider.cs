using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Basics.Files.Source
{
    /// <summary>
    /// Provides <see cref="Stream" /> instance to read the data from the source.
    /// </summary>
    public interface ISourceStreamProvider
    {
        /// <summary>
        /// Gets the identifier of the stream.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Creates a new <see cref="Stream" /> to read the data.
        /// </summary>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>Task that represents asynchronous create operation.</returns>
        Task<Stream> CreateStream(CancellationToken cancellation);
    }
}
