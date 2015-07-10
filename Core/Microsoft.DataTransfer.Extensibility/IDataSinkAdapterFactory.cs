using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Extensibility
{
    /// <summary>
    /// Provides data sink adapters capable of writing data.
    /// </summary>
    /// <typeparam name="TConfiguration">Type of the data sink adapter configuration.</typeparam>
    public interface IDataSinkAdapterFactory<in TConfiguration> : IDataAdapterFactory
    {
        /// <summary>
        /// Creates a new instance of <see cref="IDataSinkAdapter" /> with the provided configuration.
        /// </summary>
        /// <param name="configuration">Data sink adapter configuration.</param>
        /// <param name="context">Data transfer operation context.</param>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>Task that represents asynchronous create operation.</returns>
        Task<IDataSinkAdapter> CreateAsync(TConfiguration configuration, IDataTransferContext context, CancellationToken cancellation);
    }
}
