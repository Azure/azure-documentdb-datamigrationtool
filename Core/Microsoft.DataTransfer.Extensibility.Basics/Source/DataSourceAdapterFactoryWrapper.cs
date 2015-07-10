using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Extensibility.Basics.Source
{
    /// <summary>
    /// Encapsulates instance of <see cref="IDataSourceAdapterFactory{TConfiguration}" /> to hide implementation details.
    /// </summary>
    /// <typeparam name="TConfiguration">Type of the data source adapter configuration.</typeparam>
    public class DataSourceAdapterFactoryWrapper<TConfiguration> : IDataSourceAdapterFactory<TConfiguration>
    {
        /// <summary>
        /// Gets the encapsulated <see cref="IDataSourceAdapterFactory{TConfiguration}" /> instance.
        /// </summary>
        protected IDataSourceAdapterFactory<TConfiguration> Factory { get; private set; }

        /// <summary>
        /// Gets the description of the data adapter.
        /// </summary>
        public string Description
        {
            get { return Factory.Description; }
        }

        /// <summary>
        /// Creates a new instance of <see cref="DataSourceAdapterFactoryWrapper{TConfiguration}" />.
        /// </summary>
        /// <param name="factory">Instance of <see cref="IDataSourceAdapterFactory{TConfiguration}" /> to encapsulate.</param>
        public DataSourceAdapterFactoryWrapper(IDataSourceAdapterFactory<TConfiguration> factory)
        {
            Factory = factory;
        }

        /// <summary>
        /// Creates a new instance of <see cref="IDataSourceAdapter" /> with the provided configuration.
        /// </summary>
        /// <param name="configuration">Data source adapter configuration.</param>
        /// <param name="context">Data transfer operation context.</param>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>Task that represents asynchronous create operation.</returns>
        public Task<IDataSourceAdapter> CreateAsync(TConfiguration configuration, IDataTransferContext context, CancellationToken cancellation)
        {
            return Factory.CreateAsync(configuration, context, cancellation);
        }
    }
}
