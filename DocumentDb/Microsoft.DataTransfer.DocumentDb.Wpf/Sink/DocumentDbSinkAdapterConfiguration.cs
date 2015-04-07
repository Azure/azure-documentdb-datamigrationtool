using Microsoft.DataTransfer.DocumentDb.Sink;
using Microsoft.DataTransfer.DocumentDb.Wpf.Shared;
using Microsoft.DataTransfer.WpfHost.Basics.Extensions;
using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Sink
{
    abstract class DocumentDbSinkAdapterConfiguration : DocumentDbAdapterConfiguration, IDocumentDbSinkAdapterConfiguration
    {
        public static readonly string CollectionTierPropertyName =
            ObjectExtensions.MemberName<IDocumentDbSinkAdapterConfiguration>(c => c.CollectionTier);

        public static readonly string IdFieldPropertyName = 
            ObjectExtensions.MemberName<IDocumentDbSinkAdapterConfiguration>(c => c.IdField);

        public static readonly string DisableIdGenerationPropertyName =
            ObjectExtensions.MemberName<IDocumentDbSinkAdapterConfiguration>(c => c.DisableIdGeneration);

        public static readonly string DatesPropertyName = 
            ObjectExtensions.MemberName<IDocumentDbSinkAdapterConfiguration>(c => c.Dates);

        private CollectionPricingTier? collectionTier;
        private string idField;
        private bool disableIdGeneration;
        private DateTimeHandling? dates;

        public CollectionPricingTier? CollectionTier
        {
            get { return collectionTier; }
            set { SetProperty(ref collectionTier, value); }
        }

        public string IdField
        {
            get { return idField; }
            set { SetProperty(ref idField, String.IsNullOrEmpty(value) ? null : value); }
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
            CollectionTier = Defaults.Current.SinkCollectionTier;
            Dates = Defaults.Current.SinkDateTimeHandling;
        }

        protected static IReadOnlyCollection<string> ValidatePositiveInteger(int? value)
        {
            return value > 0 ? null : new[] { Resources.PositiveNumberRequired };
        }
    }
}
