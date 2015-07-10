using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Extensibility.Basics.Collections
{
    /// <summary>
    /// Represents a no-op asynchronous enumerator.
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    public sealed class EmptyAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        /// <summary>
        /// Singleton instance of the enumerator.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes",
            Justification = "Immutable singleton instance")]
        public static readonly EmptyAsyncEnumerator<T> Instance = new EmptyAsyncEnumerator<T>();

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        public T Current { get { return default(T); } }

        /// <summary>
        /// Always returns false.
        /// </summary>
        /// <returns>false</returns>
        public Task<bool> MoveNextAsync(CancellationToken cancellation)
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// Releases all associated resources.
        /// </summary>
        public void Dispose() { }
    }
}
