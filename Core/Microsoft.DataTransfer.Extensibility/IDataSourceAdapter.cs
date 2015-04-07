using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Extensibility
{
    /// <summary>
    /// Represents read-only data storage.
    /// </summary>
    public interface IDataSourceAdapter : IDisposable
    {
        /// <summary>
        /// Reads one data artifact from the storage.
        /// </summary>
        /// <param name="readOutput">
        /// Object holding additional information about the data artifact.
        /// Instance of <see cref="ReadOutputByRef" /> will be provided, implementation only need to fill in the properties.
        /// </param>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>Task that represents asynchronous read operation.</returns>
        Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation); 
    }
}
