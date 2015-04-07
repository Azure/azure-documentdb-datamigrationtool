using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Sql.Source
{
    /// <summary>
    /// Provides data source adapters capable of reading data from SQL database.
    /// </summary>
    public sealed class SqlDataSourceAdapterFactory : IDataSourceAdapterFactory<ISqlDataSourceAdapterConfiguration>
    {
        /// <summary>
        /// Gets the description of the data adapter.
        /// </summary>
        public string Description
        {
            get { return Resources.DataSourceDescription; }
        }

        /// <summary>
        /// Creates a new instance of <see cref="IDataSourceAdapter" /> with the provided configuration.
        /// </summary>
        /// <param name="configuration">Data source adapter configuration.</param>
        /// <param name="context">Data transfer operation context.</param>
        /// <returns>Task that represents asynchronous create operation.</returns>
        public async Task<IDataSourceAdapter> CreateAsync(ISqlDataSourceAdapterConfiguration configuration, IDataTransferContext context)
        {
            Guard.NotNull("configuration", configuration);

            if (String.IsNullOrEmpty(configuration.ConnectionString))
                throw Errors.ConnectionStringMissing();

            var adapter = new SqlQueryDataSourceAdapter(GetInstanceConfiguration(configuration));
            await adapter.InitializeAsync();
            return adapter;
        }

        private static ISqlDataSourceAdapterInstanceConfiguration GetInstanceConfiguration(ISqlDataSourceAdapterConfiguration configuration)
        {
            return new SqlDataSourceAdapterInstanceConfiguration
            {
                ConnectionString = configuration.ConnectionString,
                Query = GetQuery(configuration),
                NestingSeparator = configuration.NestingSeparator
            };
        }

        private static string GetQuery(ISqlDataSourceAdapterConfiguration configuration)
        {
            var isQuerySet = !String.IsNullOrEmpty(configuration.Query);
            var isQueryFileSet = !String.IsNullOrEmpty(configuration.QueryFile);

            if (isQuerySet && isQueryFileSet)
                throw Errors.AmbiguousQuery();

            var query = isQueryFileSet ? File.ReadAllText(configuration.QueryFile) : configuration.Query;

            if (String.IsNullOrEmpty(query))
                throw Errors.QueryMissing();

            return query;
        }
    }
}
