using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.DynamoDb.Client;
using Microsoft.DataTransfer.DynamoDb.Shared;
using Microsoft.DataTransfer.Extensibility;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DynamoDb.Source
{
    sealed class DynamoDbSourceAdapterInternalFactory : DynamoDbAdapterFactoryBase, IDataSourceAdapterFactory<IDynamoDbSourceAdapterConfiguration>
    {
        public string Description
        {
            get { return Resources.SourceDescription; }
        }

        public Task<IDataSourceAdapter> CreateAsync(IDynamoDbSourceAdapterConfiguration configuration, IDataTransferContext context, CancellationToken cancellation)
        {
            return Task.Factory.StartNew<IDataSourceAdapter>(Create, configuration, cancellation);
        }

        private IDataSourceAdapter Create(object state)
        {
            var configuration = state as IDynamoDbSourceAdapterConfiguration;

            Guard.NotNull("configuration", configuration);

            ValidateBaseConfiguration(configuration);

            return new DynamoDbSourceAdapter(AmazonDynamoDbFactory.Create(configuration.ConnectionString), GetInstanceConfiguration(configuration));
        }

        private IDynamoDbSourceAdapterInstanceConfiguration GetInstanceConfiguration(IDynamoDbSourceAdapterConfiguration configuration)
        {
            return new DynamoDbSourceAdapterInstanceConfiguration
            {
                Request = GetRequest(configuration)
            };
        }

        private static string GetRequest(IDynamoDbSourceAdapterConfiguration configuration)
        {
            var request = StringValueOrFile(configuration.Request, configuration.RequestFile, Errors.AmbiguousRequest);

            if (String.IsNullOrEmpty(request))
                throw Errors.RequestMissing();

            return request;
        }
    }
}
