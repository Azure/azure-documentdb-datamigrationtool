using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.DocumentDb.Sink;
using Microsoft.DataTransfer.DocumentDb.Wpf.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Sink
{
    abstract class DocumentDbSinkAdapterConfiguration : DocumentDbAdapterConfiguration, IDocumentDbSinkAdapterConfiguration
    {
        public static readonly string CollectionPropertyName =
            ObjectExtensions.MemberName<IDocumentDbSinkAdapterConfiguration>(c => c.Collection);

        private static readonly string EditableCollectionsPropertyName =
            ObjectExtensions.MemberName<DocumentDbSinkAdapterConfiguration>(c => c.EditableCollections);

        public static readonly string PartitionKeyPropertyName =
            ObjectExtensions.MemberName<IDocumentDbSinkAdapterConfiguration>(c => c.PartitionKey);

        public static readonly string CollectionTierPropertyName =
            ObjectExtensions.MemberName<IDocumentDbSinkAdapterConfiguration>(c => c.CollectionTier);

        public static readonly string IdFieldPropertyName = 
            ObjectExtensions.MemberName<IDocumentDbSinkAdapterConfiguration>(c => c.IdField);

        public static readonly string DisableIdGenerationPropertyName =
            ObjectExtensions.MemberName<IDocumentDbSinkAdapterConfiguration>(c => c.DisableIdGeneration);

        public static readonly string DatesPropertyName = 
            ObjectExtensions.MemberName<IDocumentDbSinkAdapterConfiguration>(c => c.Dates);

        private ObservableCollection<string> collections;
        private string partitionKey;
        private CollectionPricingTier? collectionTier;

        private string idField;
        private bool disableIdGeneration;

        private DateTimeHandling? dates;

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

        public DateTimeHandling? Dates
        {
            get { return dates; }
            set { SetProperty(ref dates, value); }
        }

        public DocumentDbSinkAdapterConfiguration()
        {
            EditableCollections = new ObservableCollection<string>();
            CollectionTier = Defaults.Current.SinkCollectionTier;
            Dates = Defaults.Current.SinkDateTimeHandling;
        }

        private void CollectionsListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetErrors(EditableCollectionsPropertyName, ValidateNonEmptyCollection(sender as IEnumerable<string>));
        }

        protected static IReadOnlyCollection<string> ValidatePositiveInteger(int? value)
        {
            return value > 0 ? null : new[] { Resources.PositiveNumberRequired };
        }
    }
}
