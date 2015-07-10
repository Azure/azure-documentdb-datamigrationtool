using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.RavenDb.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.RavenDb.Source
{
    sealed class RavenDbSourceAdapterInternalFactory : RavenDbAdapterFactoryBase, IDataSourceAdapterFactory<IRavenDbSourceAdapterConfiguration>
    {
        public string Description
        {
            get { return Resources.SourceDescription; }
        }

        public Task<IDataSourceAdapter> CreateAsync(IRavenDbSourceAdapterConfiguration configuration, IDataTransferContext context, CancellationToken cancellation)
        {
            return Task.Factory.StartNew(() => Create(configuration), cancellation);
        }

        private IDataSourceAdapter Create(IRavenDbSourceAdapterConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

            ValidateBaseConfiguration(configuration);

            return new RavenDbSourceAdapter(CreateInstanceConfiguration(configuration));
        }

        private IRavenDbSourceAdapterInstanceConfiguration CreateInstanceConfiguration(IRavenDbSourceAdapterConfiguration configuration)
        {
            return new RavenDbSourceAdapterInstanceConfiguration
            {
                ConnectionString = configuration.ConnectionString,
                Query = StringValueOrFile(configuration.Query, configuration.QueryFile, Errors.AmbiguousQuery),
                Index = configuration.Index ?? Defaults.Current.SourceIndex,
                ExcludeIdField = configuration.ExcludeId
            };
        }
    }
}
