using System.Linq;
using System.Threading.Tasks;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Source;

namespace Microsoft.DataTransfer.RavenDb.Source
{
    /// <summary>
    /// Provides data source adapters capable of reading data from one or more CSV files.
    /// </summary>
    public sealed class RavenDbSourceAdapterFactory : IDataSourceAdapterFactory<IRavenDbSourceAdapterConfiguration>
    {
        /// <summary>
        /// Gets the description of the data adapter.
        /// </summary>
        public string Description
        {
            get { return Resources.SourceDescription; }
        }

        /// <summary>
        /// Creates a new instance of <see cref="IDataSourceAdapter" /> with the provided configuration.
        /// </summary>
        /// <param name="configuration">Data source adapter configuration.</param>
        /// <param name="context">Data transfer operation context.</param>
        /// <returns>Task that represents asynchronous create operation.</returns>
        public Task<IDataSourceAdapter> CreateAsync(IRavenDbSourceAdapterConfiguration configuration, IDataTransferContext context)
        {
            return Task.Factory.StartNew(() => Create(configuration));
        }

        private IDataSourceAdapter Create(IRavenDbSourceAdapterConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

            return new AggregateDataSourceAdapter(
                configuration.Collections
                    .Select(c => new RavenDbSourceAdapter(GetInstanceConfiguration(c, configuration))));
        }

        private static IRavenDbSourceAdapterInstanceConfiguration GetInstanceConfiguration(string collection, IRavenDbSourceAdapterConfiguration configuration)
        {
            return new RavenDbSourceAdapterInstanceConfiguration
                {
                    ConnectionString = configuration.ConnectionString,
                    Collection = collection
                };
        }
    }
}
