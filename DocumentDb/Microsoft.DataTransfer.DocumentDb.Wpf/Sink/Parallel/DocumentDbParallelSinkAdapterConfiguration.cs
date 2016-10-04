using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.DocumentDb.Sink.Parallel;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Sink.Parallel
{
    sealed class DocumentDbParallelSinkAdapterConfiguration : DocumentDbSinkAdapterConfiguration, IDocumentDbParallelSinkAdapterConfiguration
    {
        public static readonly string CollectionPropertyName =
            ObjectExtensions.MemberName<IDocumentDbParallelSinkAdapterConfiguration>(c => c.Collection);

        public static readonly string PartitionKeyPropertyName =
            ObjectExtensions.MemberName<IDocumentDbParallelSinkAdapterConfiguration>(c => c.PartitionKey);

        public static readonly string ParallelRequestsPropertyName =
            ObjectExtensions.MemberName<IDocumentDbParallelSinkAdapterConfiguration>(c => c.ParallelRequests);

        private string collection;
        private string partitionKey;

        private int? parallelRequests;

        public string Collection
        {
            get { return collection; }
            set { SetProperty(ref collection, value, ValidateNonEmptyString); }
        }

        public string PartitionKey
        {
            get { return partitionKey; }
            set { SetProperty(ref partitionKey, value); }
        }

        public int? ParallelRequests
        {
            get { return parallelRequests; }
            set { SetProperty(ref parallelRequests, value, ValidatePositiveInteger); }
        }

        public DocumentDbParallelSinkAdapterConfiguration(ISharedDocumentDbSinkAdapterConfiguration sharedConfiguration)
            : base(sharedConfiguration)
        {
            ParallelRequests = Defaults.Current.ParallelSinkNumberOfParallelRequests;
        }
    }
}
