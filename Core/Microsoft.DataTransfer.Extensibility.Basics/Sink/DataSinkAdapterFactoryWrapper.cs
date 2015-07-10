using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Extensibility.Basics.Sink
{
    /// <summary>
    /// Encapsulates instance of <see cref="IDataSinkAdapterFactory{TConfiguration}" /> to hide implementation details.
    /// </summary>
    /// <typeparam name="TConfiguration">Type of the data sink adapter configuration.</typeparam>
    public class DataSinkAdapterFactoryWrapper<TConfiguration> : IDataSinkAdapterFactory<TConfiguration>
    {
        /// <summary>
        /// Encapsulated <see cref="IDataSinkAdapterFactory{TConfiguration}" /> instance.
        /// </summary>
        protected IDataSinkAdapterFactory<TConfiguration> Factory { get; private set; }

        /// <summary>
        /// Gets the description of the data adapter.
        /// </summary>
        public string Description
        {
            get { return Factory.Description; }
        }

        /// <summary>
        /// Creates a new instance of <see cref="DataSinkAdapterFactoryWrapper{TConfiguration}" />.
        /// </summary>
        /// <param name="factory">Instance of <see cref="IDataSinkAdapterFactory{TConfiguration}" /> to encapsulate.</param>
        public DataSinkAdapterFactoryWrapper(IDataSinkAdapterFactory<TConfiguration> factory)
        {
            Factory = factory;
        }

        /// <summary>
        /// Creates a new instance of <see cref="IDataSinkAdapter" /> with the provided configuration.
        /// </summary>
        /// <param name="configuration">Data sink adapter configuration.</param>
        /// <param name="context">Data transfer operation context.</param>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>Task that represents asynchronous create operation.</returns>
        public Task<IDataSinkAdapter> CreateAsync(TConfiguration configuration, IDataTransferContext context, CancellationToken cancellation)
        {
            return Factory.CreateAsync(configuration, context, cancellation);
        }
    }
}
