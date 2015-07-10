using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.HBase.Client;
using Microsoft.DataTransfer.HBase.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.HBase.Source
{
    sealed class HBaseSourceAdapterInternalFactory : HBaseAdapterFactoryBase, IDataSourceAdapterFactory<IHBaseSourceAdapterConfiguration>
    {
        public string Description
        {
            get { return Resources.SourceDescription; }
        }

        public async Task<IDataSourceAdapter> CreateAsync(IHBaseSourceAdapterConfiguration configuration, IDataTransferContext context, CancellationToken cancellation)
        {
            Guard.NotNull("configuration", configuration);

            ValidateBaseConfiguration(configuration);

            if (String.IsNullOrEmpty(configuration.Table))
                throw Errors.TableNameMissing();

            var adapter = new HBaseSourceAdapter(StargateClientFactory.Create(configuration.ConnectionString), GetInstanceConfiguration(configuration));

            await adapter.Initialize(cancellation);

            return adapter;
        }

        private IHBaseSourceAdapterInstanceConfiguration GetInstanceConfiguration(IHBaseSourceAdapterConfiguration configuration)
        {
            return new HBaseSourceAdapterInstanceConfiguration
            {
                TableName = configuration.Table,
                Filter = StringValueOrFile(configuration.Filter, configuration.FilterFile, Errors.AmbiguousFilter),
                ExcludeId = configuration.ExcludeId,
                BatchSize = configuration.BatchSize ?? Defaults.Current.SourceBatchSize
            };
        }
    }
}
