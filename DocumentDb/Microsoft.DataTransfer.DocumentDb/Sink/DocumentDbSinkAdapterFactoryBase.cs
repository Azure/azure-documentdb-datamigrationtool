using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Collections;
using Microsoft.DataTransfer.DocumentDb.Client;
using Microsoft.DataTransfer.DocumentDb.Shared;
using Microsoft.DataTransfer.DocumentDb.Transformation;
using Microsoft.DataTransfer.DocumentDb.Transformation.Dates;
using Microsoft.DataTransfer.DocumentDb.Transformation.Remap;
using Microsoft.DataTransfer.Extensibility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Sink
{
    abstract class DocumentDbSinkAdapterFactoryBase<TConfiguration> : DocumentDbAdapterFactoryBase, IDataSinkAdapterFactory<TConfiguration>
        where TConfiguration : IDocumentDbSinkAdapterConfiguration
    {
        private const string DocumentDbIdField = "id";

        public abstract string Description { get; }

        public Task<IDataSinkAdapter> CreateAsync(TConfiguration configuration, IDataTransferContext context)
        {
            ValidateBaseConfiguration(configuration);
            return CreateAsync(CreateClient(configuration, context), GetDataItemTransformation(configuration), configuration);
        }

        protected abstract Task<IDataSinkAdapter> CreateAsync(DocumentDbClient client, IDataItemTransformation transformation, TConfiguration configuration);

        private static IDataItemTransformation GetDataItemTransformation(IDocumentDbSinkAdapterConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

            var transformations = new List<IDataItemTransformation>();

            if (configuration.IdField != null && !DocumentDbIdField.Equals(configuration.IdField))
               transformations.Add(new RemappedFieldsTransformation(new Map<string, string> { { configuration.IdField, DocumentDbIdField } }));

            transformations.Add(new DateTimeFieldsTransformation(configuration.Dates ?? Defaults.Current.SinkDateTimeHandling));

            return new TransformationPipeline(transformations);
        }
    }
}
