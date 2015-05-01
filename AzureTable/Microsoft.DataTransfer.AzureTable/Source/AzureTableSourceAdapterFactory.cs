using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using System;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.AzureTable.Source
{
    /// <summary>
    /// Provides data source adapters capable of reading data from Azure Table storage.
    /// </summary>
    public sealed class AzureTableSourceAdapterFactory : IDataSourceAdapterFactory<IAzureTableSourceAdapterConfiguration>
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
        public Task<IDataSourceAdapter> CreateAsync(IAzureTableSourceAdapterConfiguration configuration, IDataTransferContext context)
        {
            return Task.Factory.StartNew(() => Create(configuration));
        }

        private static IDataSourceAdapter Create(IAzureTableSourceAdapterConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

            if (String.IsNullOrEmpty(configuration.ConnectionString))
                throw Errors.ConnectionStringMissing();

            return new AzureTableSourceAdapter(CreateInstanceConfiguration(configuration));
        }

        private static IAzureTableSourceAdapterInstanceConfiguration CreateInstanceConfiguration(IAzureTableSourceAdapterConfiguration configuration)
        {
            return new AzureTableSourceAdapterInstanceConfiguration
            {
                ConnectionString = configuration.ConnectionString,
                Table = configuration.Table,
                InternalFields = configuration.InternalFields ?? Defaults.Current.SourceInternalFields,
                Filter = configuration.Filter,
                Projection = configuration.Projection
            };
        }
    }
}
