using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Basics.Files.Sink
{
    /// <summary>
    /// Provides <see cref="Stream" /> instance to write the data to the target.
    /// </summary>
    public interface ISinkStreamProvider
    {
        /// <summary>
        /// Creates a new <see cref="Stream" /> to write the data.
        /// </summary>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>Task that represents asynchronous create operation.</returns>
        Task<Stream> CreateStream(CancellationToken cancellation);
    }
}
