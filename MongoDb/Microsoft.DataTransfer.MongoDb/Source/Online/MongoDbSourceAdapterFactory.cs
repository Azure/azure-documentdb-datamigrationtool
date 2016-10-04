using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.MongoDb.Source.Online
{
    /// <summary>
    /// Provides data source adapters capable of reading data from MongoDB instance.
    /// </summary>
    public sealed class MongoDbSourceAdapterFactory : DataAdapterFactoryBase, IDataSourceAdapterFactory<IMongoDbSourceAdapterConfiguration>
    {
        /// <summary>
        /// Gets the description of the data adapter.
        /// </summary>
        public string Description
        {
            get { return Resources.OnlineSourceDescription; }
        }

        /// <summary>
        /// Creates a new instance of <see cref="IDataSourceAdapter" /> with the provided configuration.
        /// </summary>
        /// <param name="configuration">Data source adapter configuration.</param>
        /// <param name="context">Data transfer operation context.</param>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>Task that represents asynchronous create operation.</returns>
        public async Task<IDataSourceAdapter> CreateAsync(IMongoDbSourceAdapterConfiguration configuration, IDataTransferContext context, CancellationToken cancellation)
        {
            Guard.NotNull("configuration", configuration);

            if (String.IsNullOrEmpty(configuration.ConnectionString))
                throw Errors.ConnectionStringMissing();

            if (String.IsNullOrEmpty(configuration.Collection))
                throw Errors.CollectionNameMissing();

            var adapter = new MongoDbSourceAdapter(GetInstanceConfiguration(configuration));
            await adapter.Initialize(cancellation);
            return adapter;
        }

        private static IMongoDbSourceAdapterInstanceConfiguration GetInstanceConfiguration(IMongoDbSourceAdapterConfiguration configuration)
        {
            return new MongoDbSourceAdapterInstanceConfiguration
            {
                ConnectionString = configuration.ConnectionString,
                Collection = configuration.Collection,
                Query = StringValueOrFile(configuration.Query, configuration.QueryFile, Errors.AmbiguousQuery),
                Projection = StringValueOrFile(configuration.Projection, configuration.ProjectionFile, Errors.AmbiguousProjection)
            };
        }
    }
}
