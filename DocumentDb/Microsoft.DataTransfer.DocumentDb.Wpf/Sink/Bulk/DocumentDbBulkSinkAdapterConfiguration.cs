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
        public static readonly string DatabasePropertyName =
            ObjectExtensions.MemberName<IDocumentDbBulkSinkAdapterConfiguration>(c => c.Database);

        public static readonly string CollectionPropertyName =
            ObjectExtensions.MemberName<IDocumentDbBulkSinkAdapterConfiguration>(c => c.Collection);

        private static readonly string EditableCollectionsPropertyName =
            ObjectExtensions.MemberName<DocumentDbBulkSinkAdapterConfiguration>(c => c.EditableCollections);

        public static readonly string PartitionKeyPropertyName =
            ObjectExtensions.MemberName<IDocumentDbBulkSinkAdapterConfiguration>(c => c.PartitionKey);

        public static readonly string StoredProcFilePropertyName =
            ObjectExtensions.MemberName<IDocumentDbBulkSinkAdapterConfiguration>(c => c.StoredProcFile);

        public static readonly string BatchSizePropertyName =
            ObjectExtensions.MemberName<IDocumentDbBulkSinkAdapterConfiguration>(c => c.BatchSize);

        public static readonly string MaxScriptSizePropertyName =
            ObjectExtensions.MemberName<IDocumentDbBulkSinkAdapterConfiguration>(c => c.MaxScriptSize);

        private string database;
        private ObservableCollection<string> collections;
        private string partitionKey;

        private string storedProcFile;
        private int? batchSize;
        private int? maxScriptSize;

        public string Database
        {
            get { return database; }
            set { SetProperty(ref database, value); }
        }

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
            BatchSize = Defaults.Current.BulkSinkBatchSize;
            MaxScriptSize = Defaults.Current.BulkSinkMaxScriptSize;
        }

        private void CollectionsListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetErrors(EditableCollectionsPropertyName, ValidateNonEmptyCollection(sender as IEnumerable<string>));
        }
    }
}
