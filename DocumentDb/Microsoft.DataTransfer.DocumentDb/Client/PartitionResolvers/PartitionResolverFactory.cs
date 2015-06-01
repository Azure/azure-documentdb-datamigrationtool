using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Partitioning;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.DocumentDb.Client.PartitionResolvers.Javascript;
using Microsoft.DataTransfer.DocumentDb.Client.PartitionResolvers.Javascript.Visitors;
using Microsoft.DataTransfer.Extensibility;
using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Client.PartitionResolvers
{
    sealed class PartitionResolverFactory
    {
        public static readonly PartitionResolverFactory Instance = new PartitionResolverFactory();

        public IPartitionResolver Create(string partitionKeyProperty, IReadOnlyList<string> collectionLinks)
        {
            if (collectionLinks.Count == 1)
            {
                return new ConstantPartitionResolver(collectionLinks[0]);
            }

            if (String.IsNullOrEmpty(partitionKeyProperty))
            {
                return new FairPartitionResolver(collectionLinks);
            }

            return new HashPartitionResolver(new PartitionKeyExtractor(partitionKeyProperty).ExtractPartitionKey, collectionLinks);
        }

        private sealed class PartitionKeyExtractor
        {
            private Func<IDataItem, object> partitionKeyExtractor;

            public PartitionKeyExtractor(string partitionKeyProperty)
            {
                Guard.NotEmpty("partitionKeyProperty", partitionKeyProperty);

                var evaluationVisitor = new DataItemMemberEvaluationVisitor();
                new JavascriptMemberExpression(partitionKeyProperty).Accept(evaluationVisitor);
                partitionKeyExtractor = evaluationVisitor.GetAccessor();
            }

            public string ExtractPartitionKey(object document)
            {
                var dataItem = document as IDataItem;

                if (dataItem == null)
                    throw Errors.FailedToExtractPartitionKey(Resources.DocumentIsNotDataItem);

                object partitionKey = null;
                try
                {
                    partitionKey = partitionKeyExtractor(dataItem);
                }
                catch (Exception error)
                {
                    throw Errors.FailedToExtractPartitionKey(error.Message);
                }

                return partitionKey == null ? null : partitionKey.ToString();
            }
        }
    }
}
