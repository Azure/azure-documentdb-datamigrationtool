using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.DocumentDb.Shared;
using Microsoft.DataTransfer.DocumentDb.Transformation;
using Microsoft.DataTransfer.DocumentDb.Transformation.Filter;
using Microsoft.DataTransfer.Extensibility;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Source
{
    sealed class DocumentDbSourceAdapterInternalFactory : DocumentDbAdapterFactoryBase, IDataSourceAdapterFactory<IDocumentDbSourceAdapterConfiguration>
    {
        private const string DefaultQuery = "SELECT * FROM c";

        private static readonly string[] DocumentDbInternalFields = new[] { "_rid", "_ts", "_self", "_etag", "_attachments" };

        public string Description
        {
            get { return Resources.SourceDescription; }
        }

        public Task<IDataSourceAdapter> CreateAsync(IDocumentDbSourceAdapterConfiguration configuration, IDataTransferContext context)
        {
            return Task.Factory.StartNew<IDataSourceAdapter>(() => Create(configuration, context));
        }

        private IDataSourceAdapter Create(IDocumentDbSourceAdapterConfiguration configuration, IDataTransferContext context)
        {
            Guard.NotNull("configuration", configuration);

            ValidateBaseConfiguration(configuration);

            var source = new DocumentDbSourceAdapter(
                CreateClient(configuration, context),
                GetDataItemTransformation(configuration),
                GetInstanceConfiguration(configuration));

            source.Initialize();

            return source;
        }

        private static IDataItemTransformation GetDataItemTransformation(IDocumentDbSourceAdapterConfiguration configuration)
        {
            if (configuration.InternalFields)
                return PassThroughTransformation.Instance;

            return new FilteredFieldsTransformation(DocumentDbInternalFields);
        }

        private static DocumentDbSourceAdapterInstanceConfiguration GetInstanceConfiguration(IDocumentDbSourceAdapterConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

            return new DocumentDbSourceAdapterInstanceConfiguration
            {
                CollectionName = configuration.Collection,
                Query = GetQuery(configuration)
            };
        }

        private static string GetQuery(IDocumentDbSourceAdapterConfiguration configuration)
        {
            var isQuerySet = !String.IsNullOrEmpty(configuration.Query);
            var isQueryFileSet = !String.IsNullOrEmpty(configuration.QueryFile);

            if (isQuerySet && isQueryFileSet)
                throw Errors.AmbiguousQuery();

            var query = isQueryFileSet ? File.ReadAllText(configuration.QueryFile) : configuration.Query;

            if (String.IsNullOrEmpty(query))
                query = DefaultQuery;

            return query;
        }
    }
}
