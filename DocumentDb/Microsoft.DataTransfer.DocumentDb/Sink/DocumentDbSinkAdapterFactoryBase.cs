using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Collections;
using Microsoft.DataTransfer.DocumentDb.Client;
using Microsoft.DataTransfer.DocumentDb.Shared;
using Microsoft.DataTransfer.DocumentDb.Sink.Substitutions;
using Microsoft.DataTransfer.DocumentDb.Sink.Substitutions.Range;
using Microsoft.DataTransfer.DocumentDb.Transformation;
using Microsoft.DataTransfer.DocumentDb.Transformation.Dates;
using Microsoft.DataTransfer.DocumentDb.Transformation.Remap;
using Microsoft.DataTransfer.DocumentDb.Transformation.Stringify;
using Microsoft.DataTransfer.Extensibility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Sink
{
    abstract class DocumentDbSinkAdapterFactoryBase<TConfiguration> : DocumentDbAdapterFactoryBase, IDataSinkAdapterFactory<TConfiguration>
        where TConfiguration : IDocumentDbSinkAdapterConfiguration
    {
        private const string DocumentDbIdField = "id";

        private static readonly ISubstitutionResolver substitutions = new RangeSubstitutionResolver();

        public abstract string Description { get; }

        public Task<IDataSinkAdapter> CreateAsync(TConfiguration configuration, IDataTransferContext context)
        {
            ValidateBaseConfiguration(configuration);

            var collectionNames = ResolveCollectionNames(configuration.Collection);
            if (!collectionNames.Any())
                throw Errors.CollectionNameMissing();

            return CreateAsync(
                CreateClient(configuration, context, collectionNames.Length > 1),
                GetDataItemTransformation(configuration),
                configuration, collectionNames);
        }

        protected abstract Task<IDataSinkAdapter> CreateAsync(DocumentDbClient client, IDataItemTransformation transformation,
            TConfiguration configuration, IEnumerable<string> collectionNames);

        private static IDataItemTransformation GetDataItemTransformation(IDocumentDbSinkAdapterConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

            var transformations = new List<IDataItemTransformation>();

            if (configuration.IdField != null && !DocumentDbIdField.Equals(configuration.IdField))
               transformations.Add(new RemapFieldsTransformation(new Map<string, string> { { configuration.IdField, DocumentDbIdField } }));

            transformations.Add(new DateTimeFieldsTransformation(configuration.Dates ?? Defaults.Current.SinkDateTimeHandling));

            transformations.Add(new StringifyFieldsTransformation(new[] { DocumentDbIdField }));

            return new TransformationPipeline(transformations);
        }

        private static string[] ResolveCollectionNames(IEnumerable<string> collectionNamePatterns)
        {
            return collectionNamePatterns.AsParallel().SelectMany(p => substitutions.Resolve(p)).Distinct().ToArray();
        }
    }
}
