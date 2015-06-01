using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.RavenDb.Shared;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.RavenDb.Source
{
    sealed class RavenDbSourceAdapterInternalFactory : RavenDbAdapterFactoryBase, IDataSourceAdapterFactory<IRavenDbSourceAdapterConfiguration>
    {
        public string Description
        {
            get { return Resources.SourceDescription; }
        }

        public Task<IDataSourceAdapter> CreateAsync(IRavenDbSourceAdapterConfiguration configuration, IDataTransferContext context)
        {
            return Task.Factory.StartNew(() => Create(configuration));
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
                Query = GetQuery(configuration),
                Index = configuration.Index ?? Defaults.Current.SourceIndex,
                ExcludeIdField = configuration.ExcludeId
            };
        }

        private static string GetQuery(IRavenDbSourceAdapterConfiguration configuration)
        {
            var isQuerySet = !String.IsNullOrEmpty(configuration.Query);
            var isQueryFileSet = !String.IsNullOrEmpty(configuration.QueryFile);

            if (isQuerySet && isQueryFileSet)
                throw Errors.AmbiguousQuery();

            var query = isQueryFileSet ? File.ReadAllText(configuration.QueryFile) : configuration.Query;

            return query;
        }
    }
}
