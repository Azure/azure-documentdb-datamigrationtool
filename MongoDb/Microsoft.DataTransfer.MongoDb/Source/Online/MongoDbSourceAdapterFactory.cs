using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.MongoDb.Source.Online
{
    /// <summary>
    /// Provides data source adapters capable of reading data from MongoDB instance.
    /// </summary>
    public sealed class MongoDbSourceAdapterFactory : IDataSourceAdapterFactory<IMongoDbSourceAdapterConfiguration>
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
        /// <returns>Task that represents asynchronous create operation.</returns>
        public Task<IDataSourceAdapter> CreateAsync(IMongoDbSourceAdapterConfiguration configuration, IDataTransferContext context)
        {
            return Task.Factory.StartNew(() => Create(configuration));
        }

        private IDataSourceAdapter Create(IMongoDbSourceAdapterConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

            if (String.IsNullOrEmpty(configuration.ConnectionString))
                throw Errors.ConnectionStringMissing();

            if (String.IsNullOrEmpty(configuration.Collection))
                throw Errors.CollectionNameMissing();

            if (!String.IsNullOrEmpty(configuration.QueryFile) && !String.IsNullOrEmpty(configuration.Query))
                throw Errors.AmbiguousQuery();

            if (!String.IsNullOrEmpty(configuration.ProjectionFile) && !String.IsNullOrEmpty(configuration.Projection))
                throw Errors.AmbiguousProjection();

            var adapter = new MongoDbSourceAdapter(GetInstanceConfiguration(configuration));
            adapter.Initialize();
            return adapter;
        }

        private static IMongoDbSourceAdapterInstanceConfiguration GetInstanceConfiguration(IMongoDbSourceAdapterConfiguration configuration)
        {
            return new MongoDbSourceAdapterInstanceConfiguration
            {
                ConnectionString = configuration.ConnectionString,
                Collection = configuration.Collection,
                Query = String.IsNullOrEmpty(configuration.QueryFile) ? 
                    configuration.Query : File.ReadAllText(configuration.QueryFile),
                Projection = String.IsNullOrEmpty(configuration.ProjectionFile) ? 
                    configuration.Projection : File.ReadAllText(configuration.ProjectionFile)
            };
        }
    }
}
