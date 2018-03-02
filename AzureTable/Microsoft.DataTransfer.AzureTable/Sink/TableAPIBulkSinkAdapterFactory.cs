namespace Microsoft.DataTransfer.TableAPI.Sink.Bulk
{
    using Microsoft.DataTransfer.AzureTable;
    using Microsoft.DataTransfer.Basics;
    using Microsoft.DataTransfer.Extensibility;
    using Microsoft.DataTransfer.Extensibility.Basics;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides data sink adapters capable of writing data to Azure CosmosDB tables.
    /// </summary>
    public sealed class TableAPIBulkSinkAdapterFactory : DataAdapterFactoryBase, IDataSinkAdapterFactory<ITableAPIBulkSinkAdapterConfiguration>
    {
        /// <summary>
        /// Gets the description of the data adapter.
        /// </summary>
        public string Description
        {
            get { return Resources.BulkSinkDescription; }
        }

        /// <summary>
        /// Creates a new instance of <see cref="IDataSinkAdapter" /> with the provided configuration.
        /// </summary>
        /// <param name="configuration">Data sink adapter configuration.</param>
        /// <param name="context">Data transfer operation context.</param>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>Task that represents asynchronous create operation.</returns>
        public async Task<IDataSinkAdapter> CreateAsync(ITableAPIBulkSinkAdapterConfiguration configuration, IDataTransferContext context, CancellationToken cancellation)
        {
            Guard.NotNull("configuration", configuration);

            if (String.IsNullOrEmpty(configuration.ConnectionString))
                throw Errors.ConnectionStringMissing();

            if (String.IsNullOrEmpty(configuration.TableName))
                throw Errors.TableNameMissing();

            long maxInputBufferSizeInBytes = 10 * 1024 * 1024;
            if (configuration.MaxInputBufferSize.HasValue)
                maxInputBufferSizeInBytes = configuration.MaxInputBufferSize.Value;

            int throughput = 400;
            if (configuration.Throughput.HasValue)
                throughput = configuration.Throughput.Value;

            const int maxBatchSize = 2 * 1024 * 1024;
            int batchSize = maxBatchSize;
            if (configuration.MaxBatchSize.HasValue)
            {
                if (configuration.MaxBatchSize.Value > maxBatchSize)
                    throw Errors.BatchSizeInvalid(maxBatchSize);

                batchSize = configuration.MaxBatchSize.Value;
            }

            var sink = new TableAPIBulkSinkAdapter(configuration.ConnectionString, 
                            configuration.TableName, configuration.Overwrite, 
                            maxInputBufferSizeInBytes, throughput, batchSize);

            await sink.InitializeAsync(cancellation);

            return sink;
        }
    }
}
