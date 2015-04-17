using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;

namespace Microsoft.DataTransfer.RavenDb.Shared
{
    abstract class RavenDbDataAdapterBase<TConfiguration> : IDataSourceAdapter
        where TConfiguration : class, IRavenDbDataAdapterConfiguration
    {
        private Lazy<IDocumentStore> documentStore;

        protected TConfiguration Configuration { get; private set; }

        protected IDocumentStore Connection
        {
            get { return documentStore.Value; }
        }

        public RavenDbDataAdapterBase(TConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

            Configuration = configuration;
            documentStore = new Lazy<IDocumentStore>(CreateConnection);
        }

        private IDocumentStore CreateConnection()
        {
            var connectionStringOptions = ConnectionStringParser<RavenConnectionStringOptions>.FromConnectionString(Configuration.ConnectionString);
            connectionStringOptions.Parse();

            var options = connectionStringOptions.ConnectionStringOptions;

            var localDocumentStore = new DocumentStore();


            if (options.ResourceManagerId != Guid.Empty)
                localDocumentStore.ResourceManagerId = options.ResourceManagerId;
            if (options.Credentials != null)
                localDocumentStore.Credentials = options.Credentials;
            if (string.IsNullOrEmpty(options.Url) == false)
                localDocumentStore.Url = options.Url;
            if (string.IsNullOrEmpty(options.DefaultDatabase) == false)
                localDocumentStore.DefaultDatabase = options.DefaultDatabase;
            if (string.IsNullOrEmpty(options.ApiKey) == false)
                localDocumentStore.ApiKey = options.ApiKey;

            localDocumentStore.EnlistInDistributedTransactions = options.EnlistInDistributedTransactions;

            localDocumentStore.Initialize();
            return localDocumentStore;
        }

        public abstract Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation);

        public virtual void Dispose()
        {
            if (documentStore != null && documentStore.IsValueCreated && documentStore.Value != null)
            {
                documentStore.Value.Dispose();
                documentStore = null;
            }
        }
    }
}
