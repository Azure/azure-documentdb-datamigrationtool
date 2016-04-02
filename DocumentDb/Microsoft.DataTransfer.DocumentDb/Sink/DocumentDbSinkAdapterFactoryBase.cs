using Microsoft.Azure.Documents;
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.Sink
{
    abstract class DocumentDbSinkAdapterFactoryBase<TConfiguration> : DocumentDbAdapterFactoryBase, IDataSinkAdapterFactory<TConfiguration>
        where TConfiguration : IDocumentDbSinkAdapterConfiguration
    {
        private const string DocumentDbIdField = "id";

        public abstract string Description { get; }

        public Task<IDataSinkAdapter> CreateAsync(TConfiguration configuration, IDataTransferContext context, CancellationToken cancellation)
        {
            ValidateBaseConfiguration(configuration);

            return CreateAsync(
                context, GetDataItemTransformation(configuration),
                configuration, cancellation);
        }

        protected abstract Task<IDataSinkAdapter> CreateAsync(IDataTransferContext context, IDataItemTransformation transformation,
            TConfiguration configuration, CancellationToken cancellation);

        protected static IndexingPolicy GetIndexingPolicy(TConfiguration configuration)
        {
            var policyJson = StringValueOrFile(configuration.IndexingPolicy, configuration.IndexingPolicyFile, Errors.AmbiguousIndexingPolicy);

            if (String.IsNullOrEmpty(policyJson))
                return null;

            return JsonConvert.DeserializeObject<IndexingPolicy>(policyJson);
        }

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
    }
}
