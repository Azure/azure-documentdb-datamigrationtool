using Microsoft.DataTransfer.DocumentDb.Sink;
using Microsoft.DataTransfer.DocumentDb.Wpf.Shared;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Sink
{
    sealed class SharedDocumentDbSinkAdapterConfiguration : SharedDocumentDbAdapterConfiguration, ISharedDocumentDbSinkAdapterConfiguration
    {
        private int? collectionThroughput;

        private bool useIndexingPolicyFile;
        private string indexingPolicy;
        private string indexingPolicyFile;

        private string idField;
        private bool disableIdGeneration;
        private bool updateExisting;

        private DateTimeHandling? dates;

        public int? CollectionThroughput
        {
            get { return collectionThroughput; }
            set { SetProperty(ref collectionThroughput, value, ValidateNonNegativeInteger); }
        }

        public bool UseIndexingPolicyFile
        {
            get { return useIndexingPolicyFile; }
            set { SetProperty(ref useIndexingPolicyFile, value); }
        }

        public string IndexingPolicy
        {
            get { return useIndexingPolicyFile ? null : indexingPolicy; }
            set { SetProperty(ref indexingPolicy, value); }
        }

        public string IndexingPolicyFile
        {
            get { return useIndexingPolicyFile ? indexingPolicyFile : null; }
            set { SetProperty(ref indexingPolicyFile, value); }
        }

        public string IdField
        {
            get { return idField; }
            set { SetProperty(ref idField, value); }
        }

        public bool DisableIdGeneration
        {
            get { return disableIdGeneration; }
            set { SetProperty(ref disableIdGeneration, value); }
        }

        public bool UpdateExisting
        {
            get { return updateExisting; }
            set { SetProperty(ref updateExisting, value); }
        }

        public DateTimeHandling? Dates
        {
            get { return dates; }
            set { SetProperty(ref dates, value); }
        }

        public SharedDocumentDbSinkAdapterConfiguration()
        {
            CollectionThroughput = Defaults.Current.SinkCollectionThroughput;
            Dates = Defaults.Current.SinkDateTimeHandling;
        }
    }
}
