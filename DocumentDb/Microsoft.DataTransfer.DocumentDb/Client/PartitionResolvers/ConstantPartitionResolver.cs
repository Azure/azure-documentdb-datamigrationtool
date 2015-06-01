using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Client.PartitionResolvers
{
    sealed class ConstantPartitionResolver : IPartitionResolver
    {
        private readonly string collectionName;

        public ConstantPartitionResolver(string collectionName)
        {
            this.collectionName = collectionName;
        }

        public object GetPartitionKey(object document)
        {
            return null;
        }

        public string ResolveForCreate(object partitionKey)
        {
            return collectionName;
        }

        public IEnumerable<string> ResolveForRead(object partitionKey)
        {
            return new[] { collectionName };
        }
    }
}
