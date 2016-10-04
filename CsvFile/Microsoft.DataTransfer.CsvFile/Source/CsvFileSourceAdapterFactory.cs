using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Files.Source;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.CsvFile.Source
{
    /// <summary>
    /// Provides data source adapters capable of reading data from one or more CSV files.
    /// </summary>
    public sealed class CsvFileSourceAdapterFactory : DataAdapterFactoryBase, IDataSourceAdapterFactory<ICsvFileSourceAdapterConfiguration>
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
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>Task that represents asynchronous create operation.</returns>
        public Task<IDataSourceAdapter> CreateAsync(ICsvFileSourceAdapterConfiguration configuration, IDataTransferContext context, CancellationToken cancellation)
        {
            return Task.Factory.StartNew(() => Create(configuration), cancellation);
        }

        private IDataSourceAdapter Create(ICsvFileSourceAdapterConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

            var instanceConfiguration = GetInstanceConfiguration(configuration);

            return new AggregateDataSourceAdapter(
                configuration.Files
                    .SelectMany(f => SourceStreamProvidersFactory
                        .Create(f, configuration.Decompress)
                        .Select(p => new CsvFileSourceAdapter(p, instanceConfiguration))));
        }

        private static ICsvFileSourceAdapterInstanceConfiguration GetInstanceConfiguration(ICsvFileSourceAdapterConfiguration configuration)
        {
            return new CsvFileSourceAdapterInstanceConfiguration
            {
                NestingSeparator = configuration.NestingSeparator,
                TrimQuoted = configuration.TrimQuoted,
                NoUnquotedNulls = configuration.NoUnquotedNulls,
                UseRegionalSettings = configuration.UseRegionalSettings
            };
        }
    }
}
