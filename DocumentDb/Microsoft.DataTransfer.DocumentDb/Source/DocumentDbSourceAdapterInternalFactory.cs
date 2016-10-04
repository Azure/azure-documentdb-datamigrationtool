using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.DocumentDb.Shared;
using Microsoft.DataTransfer.DocumentDb.Transformation;
using Microsoft.DataTransfer.DocumentDb.Transformation.Filter;
using Microsoft.DataTransfer.Extensibility;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Source
{
    sealed class DocumentDbSourceAdapterInternalFactory : DocumentDbAdapterFactoryBase, IDataSourceAdapterFactory<IDocumentDbSourceAdapterConfiguration>
    {
        private static readonly string[] DocumentDbInternalFields = new[] { "_rid", "_ts", "_self", "_etag", "_attachments" };

        public string Description
        {
            get { return Resources.SourceDescription; }
        }

        public async Task<IDataSourceAdapter> CreateAsync(IDocumentDbSourceAdapterConfiguration configuration, IDataTransferContext context, CancellationToken cancellation)
        {
            Guard.NotNull("configuration", configuration);

            ValidateBaseConfiguration(configuration);

            var source = new DocumentDbSourceAdapter(
                CreateClient(configuration, context, false, null),
                GetDataItemTransformation(configuration),
                GetInstanceConfiguration(configuration));

            await source.InitializeAsync(cancellation);

            return source;
        }

        private static IDataItemTransformation GetDataItemTransformation(IDocumentDbSourceAdapterConfiguration configuration)
        {
            if (configuration.InternalFields)
                return PassThroughTransformation.Instance;

            return new FilterFieldsTransformation(DocumentDbInternalFields);
        }

        private static DocumentDbSourceAdapterInstanceConfiguration GetInstanceConfiguration(IDocumentDbSourceAdapterConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

            if (String.IsNullOrEmpty(configuration.Collection))
                throw Errors.CollectionNameMissing();

            return new DocumentDbSourceAdapterInstanceConfiguration
            {
                Collection = configuration.Collection,
                Query = GetQuery(configuration)
            };
        }

        private static string GetQuery(IDocumentDbSourceAdapterConfiguration configuration)
        {
            var isQuerySet = !String.IsNullOrEmpty(configuration.Query);
            var isQueryFileSet = !String.IsNullOrEmpty(configuration.QueryFile);

            if (isQuerySet && isQueryFileSet)
                throw Errors.AmbiguousQuery();

            return isQueryFileSet ? File.ReadAllText(configuration.QueryFile) : configuration.Query;
        }
    }
}
