using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Extensibility
{
    /// <summary>
    /// Represents write-only data storage.
    /// </summary>
    public interface IDataSinkAdapter : IDisposable
    {
        /// <summary>
        /// Gets number of maximum supported parallel write operations.
        /// </summary>
        int MaxDegreeOfParallelism { get; }

        /// <summary>
        /// Writes one data artifact to the storage.
        /// </summary>
        /// <param name="dataItem">Data artifact to write.</param>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>Task that represents asynchronous write operation.</returns>
        Task WriteAsync(IDataItem dataItem, CancellationToken cancellation);

        /// <summary>
        /// Signals that no more data artifacts will be sent.
        /// </summary>
        /// <remarks>
        /// Call to this method can occur before all write tasks (returned by WriteAsync) have completed,
        /// thus no resources should be disposed in this method. Instead, this method is useful when
        /// implementing batch operations, e.g. last batch can be submitted to the underlying storage.
        /// </remarks>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>Task that represents asynchronous write operation.</returns>
        Task CompleteAsync(CancellationToken cancellation);
    }
}
