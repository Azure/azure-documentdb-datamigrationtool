using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.DocumentDb.Sink;
using Microsoft.DataTransfer.DocumentDb.Sink.Bulk;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Sink.Bulk
{
    sealed class DocumentDbBulkSinkAdapterConfiguration : DocumentDbSinkAdapterConfiguration, IDocumentDbBulkSinkAdapterConfiguration
    {
        public static readonly string CollectionPropertyName =
            ObjectExtensions.MemberName<IDocumentDbBulkSinkAdapterConfiguration>(c => c.Collection);

        private static readonly string EditableCollectionsPropertyName =
            ObjectExtensions.MemberName<DocumentDbBulkSinkAdapterConfiguration>(c => c.EditableCollections);

        public static readonly string PartitionKeyPropertyName =
            ObjectExtensions.MemberName<IDocumentDbBulkSinkAdapterConfiguration>(c => c.PartitionKey);

        public static readonly string CollectionTierPropertyName =
            ObjectExtensions.MemberName<IDocumentDbBulkSinkAdapterConfiguration>(c => c.CollectionTier);

        public static readonly string StoredProcFilePropertyName =
            ObjectExtensions.MemberName<IDocumentDbBulkSinkAdapterConfiguration>(c => c.StoredProcFile);

        public static readonly string BatchSizePropertyName =
            ObjectExtensions.MemberName<IDocumentDbBulkSinkAdapterConfiguration>(c => c.BatchSize);

        public static readonly string MaxScriptSizePropertyName =
            ObjectExtensions.MemberName<IDocumentDbBulkSinkAdapterConfiguration>(c => c.MaxScriptSize);

        private ObservableCollection<string> collections;
        private string partitionKey;
        private CollectionPricingTier? collectionTier;

        private string storedProcFile;
        private int? batchSize;
        private int? maxScriptSize;

        public IEnumerable<string> Collection
        {
            get { return collections; }
        }

        public ObservableCollection<string> EditableCollections
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

        public string StoredProcFile
        {
            get { return storedProcFile; }
            set { SetProperty(ref storedProcFile, value); }
        }

        public int? BatchSize
        {
            get { return batchSize; }
            set { SetProperty(ref batchSize, value, ValidatePositiveInteger); }
        }

        public int? MaxScriptSize
        {
            get { return maxScriptSize; }
            set { SetProperty(ref maxScriptSize, value, ValidatePositiveInteger); }
        }

        public DocumentDbBulkSinkAdapterConfiguration(ISharedDocumentDbSinkAdapterConfiguration sharedConfiguration)
            : base(sharedConfiguration)
        {
            EditableCollections = new ObservableCollection<string>();
            CollectionTier = Defaults.Current.BulkSinkCollectionTier;
            BatchSize = Defaults.Current.BulkSinkBatchSize;
            MaxScriptSize = Defaults.Current.BulkSinkMaxScriptSize;
        }

        private void CollectionsListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetErrors(EditableCollectionsPropertyName, ValidateNonEmptyCollection(sender as IEnumerable<string>));
        }
    }
}
