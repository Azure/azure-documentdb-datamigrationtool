using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Extensibility.Basics.Collections
{
    /// <summary>
    /// Supports an asynchronous iteration over a generic collection.
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    public interface IAsyncEnumerator<out T> : IDisposable
    {
        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        T Current { get; }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>
        /// true if the enumerator was successfully advanced to the next element; false
        /// if the enumerator has passed the end of the collection.
        /// </returns>
        Task<bool> MoveNextAsync(CancellationToken cancellation);
    }
}
