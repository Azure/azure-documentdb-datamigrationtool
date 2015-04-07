using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.CsvFile.Source
{
    /// <summary>
    /// Provides data source adapters capable of reading data from one or more CSV files.
    /// </summary>
    public sealed class CsvFileSourceAdapterFactory : IDataSourceAdapterFactory<ICsvFileSourceAdapterConfiguration>
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
        public Task<IDataSourceAdapter> CreateAsync(ICsvFileSourceAdapterConfiguration configuration, IDataTransferContext context)
        {
            return Task.Factory.StartNew(() => Create(configuration));
        }

        private IDataSourceAdapter Create(ICsvFileSourceAdapterConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

            return new AggregateDataSourceAdapter(
                configuration.Files
                    .SelectMany(p => DirectoryHelper
                        .EnumerateFiles(p)
                        .Select(f => new CsvFileSourceAdapter(GetInstanceConfiguration(f, configuration)))));
        }

        private static ICsvFileSourceAdapterInstanceConfiguration GetInstanceConfiguration(string fileName, ICsvFileSourceAdapterConfiguration configuration)
        {
            return new CsvFileSourceAdapterInstanceConfiguration
            {
                FileName = fileName,
                NestingSeparator = configuration.NestingSeparator
            };
        }
    }
}
