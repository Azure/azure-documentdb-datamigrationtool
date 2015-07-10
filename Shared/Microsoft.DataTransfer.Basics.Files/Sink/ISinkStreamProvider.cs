using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Basics.Files.Sink
{
    /// <summary>
    /// Provides <see cref="StreamWriter" /> instance to write data to the target stream.
    /// </summary>
    public interface ISinkStreamProvider
    {
        /// <summary>
        /// Creates a new <see cref="StreamWriter" /> to write data to the stream.
        /// </summary>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>Task that represents asynchronous create operation.</returns>
        Task<StreamWriter> CreateWriter(CancellationToken cancellation);
    }
}
