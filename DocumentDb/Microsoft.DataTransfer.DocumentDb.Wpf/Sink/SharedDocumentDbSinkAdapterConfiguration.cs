using Microsoft.DataTransfer.DocumentDb.Sink;
using Microsoft.DataTransfer.DocumentDb.Wpf.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Sink
{
    sealed class SharedDocumentDbSinkAdapterConfiguration : SharedDocumentDbAdapterConfiguration, ISharedDocumentDbSinkAdapterConfiguration
    {
        private ObservableCollection<string> collections;
        private string partitionKey;
        private CollectionPricingTier? collectionTier;

        private bool useIndexingPolicyFile;
        private string indexingPolicy;
        private string indexingPolicyFile;

        private string idField;
        private bool disableIdGeneration;
        private bool updateExisting;

        private DateTimeHandling? dates;

        public ObservableCollection<string> Collections
        {
            get { return collections; }
            private set
            {
                if (collections != null)
                    collections.CollectionChanged -= CollectionsListChanged;

                SetProperty(ref collections, value, ValidateNonEmptyCollection);

                if (collections != null)
                    collections.CollectionChanged += CollectionsListChanged;
            }
        }

        public string PartitionKey
        {
            get { return partitionKey; }
            set { SetProperty(ref partitionKey, value); }
        }

        public CollectionPricingTier? CollectionTier
        {
            get { return collectionTier; }
            set { SetProperty(ref collectionTier, value); }
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
            Collections = new ObservableCollection<string>();
            CollectionTier = Defaults.Current.SinkCollectionTier;
            Dates = Defaults.Current.SinkDateTimeHandling;
        }

        private void CollectionsListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetErrors(SharedDocumentDbSinkAdapterConfigurationProperties.Collections,
                ValidateNonEmptyCollection(sender as IEnumerable<string>));
        }
    }
}
