using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Extensibility
{
    /// <summary>
    /// Provides data source adapters capable of reading data.
    /// </summary>
    /// <typeparam name="TConfiguration">Type of the data source adapter configuration.</typeparam>
    public interface IDataSourceAdapterFactory<in TConfiguration> : IDataAdapterFactory
    {
        /// <summary>
        /// Creates a new instance of <see cref="IDataSourceAdapter" /> with the provided configuration.
        /// </summary>
        /// <param name="configuration">Data source adapter configuration.</param>
        /// <param name="context">Data transfer operation context.</param>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>Task that represents asynchronous create operation.</returns>
        Task<IDataSourceAdapter> CreateAsync(TConfiguration configuration, IDataTransferContext context, CancellationToken cancellation);
    }
}
