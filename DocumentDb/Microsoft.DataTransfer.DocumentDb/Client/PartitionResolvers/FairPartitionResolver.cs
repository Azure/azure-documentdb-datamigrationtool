using Microsoft.Azure.Documents.Client;
using Microsoft.DataTransfer.Basics;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.DataTransfer.DocumentDb.Client.PartitionResolvers
{
    sealed class FairPartitionResolver : IPartitionResolver, IDisposable
    {
        private readonly IReadOnlyList<string> collectionLinks;
        private ThreadLocal<Random> random;

        public FairPartitionResolver(IReadOnlyList<string> collectionLinks)
        {
            Guard.NotNull("collectionLinks", collectionLinks);

            this.collectionLinks = collectionLinks;
            this.random = new ThreadLocal<Random>(CreateNewRandom);
        }

        private static Random CreateNewRandom()
        {
            return new Random();
        }

        public object GetPartitionKey(object document)
        {
            return null;
        }

        public string ResolveForCreate(object partitionKey)
        {
            return collectionLinks[random.Value.Next(collectionLinks.Count)];
        }

        public IEnumerable<string> ResolveForRead(object partitionKey)
        {
            return collectionLinks;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "random",
            Justification = "Disposed through TrashCan helper")]
        public void Dispose()
        {
            TrashCan.Throw(ref random);
        }
    }
}
