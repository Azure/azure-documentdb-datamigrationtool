using Microsoft.DataTransfer.Basics.Collections;
using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.DocumentDb.Sink;
using Microsoft.DataTransfer.DocumentDb.Wpf.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Sink
{
    abstract class DocumentDbSinkAdapterConfiguration : DocumentDbAdapterConfiguration<ISharedDocumentDbSinkAdapterConfiguration>, IDocumentDbSinkAdapterConfiguration
    {
        public static readonly string CollectionThroughputPropertyName =
            ObjectExtensions.MemberName<IDocumentDbSinkAdapterConfiguration>(c => c.CollectionThroughput);

        public static readonly string IndexingPolicyPropertyName =
            ObjectExtensions.MemberName<IDocumentDbSinkAdapterConfiguration>(c => c.IndexingPolicy);

        public static readonly string IndexingPolicyFilePropertyName =
            ObjectExtensions.MemberName<IDocumentDbSinkAdapterConfiguration>(c => c.IndexingPolicyFile);

        public static readonly string IdFieldPropertyName = 
            ObjectExtensions.MemberName<IDocumentDbSinkAdapterConfiguration>(c => c.IdField);

        public static readonly string DisableIdGenerationPropertyName =
            ObjectExtensions.MemberName<IDocumentDbSinkAdapterConfiguration>(c => c.DisableIdGeneration);

        public static readonly string UpdateExistingPropertyName =
            ObjectExtensions.MemberName<IDocumentDbSinkAdapterConfiguration>(c => c.UpdateExisting);

        public static readonly string DatesPropertyName = 
            ObjectExtensions.MemberName<IDocumentDbSinkAdapterConfiguration>(c => c.Dates);

        private static readonly string UseIndexingPolicyFilePropertyName = 
            ObjectExtensions.MemberName<DocumentDbSinkAdapterConfiguration>(c => c.UseIndexingPolicyFile);

        public int? CollectionThroughput
        {
            get { return SharedConfiguration.CollectionThroughput; }
            set { SharedConfiguration.CollectionThroughput = value; }
        }

        public bool UseIndexingPolicyFile
        {
            get { return SharedConfiguration.UseIndexingPolicyFile; }
            set { SharedConfiguration.UseIndexingPolicyFile = value; }
        }

        public string IndexingPolicy
        {
            get { return SharedConfiguration.IndexingPolicy; }
            set { SharedConfiguration.IndexingPolicy = value; }
        }

        public string IndexingPolicyFile
        {
            get { return SharedConfiguration.IndexingPolicyFile; }
            set { SharedConfiguration.IndexingPolicyFile = value; }
        }

        public string IdField
        {
            get { return SharedConfiguration.IdField; }
            set { SharedConfiguration.IdField = value; }
        }

        public bool DisableIdGeneration
        {
            get { return SharedConfiguration.DisableIdGeneration; }
            set { SharedConfiguration.DisableIdGeneration = value; }
        }

        public bool UpdateExisting
        {
            get { return SharedConfiguration.UpdateExisting; }
            set { SharedConfiguration.UpdateExisting = value; }
        }

        public DateTimeHandling? Dates
        {
            get { return SharedConfiguration.Dates; }
            set { SharedConfiguration.Dates = value; }
        }

        public DocumentDbSinkAdapterConfiguration(ISharedDocumentDbSinkAdapterConfiguration sharedConfiguration)
            : base(sharedConfiguration) { }

        protected override Map<string, string> GetSharedPropertiesMapping()
        {
            var mapping = base.GetSharedPropertiesMapping();
            mapping.Add(SharedDocumentDbSinkAdapterConfigurationProperties.CollectionThroughput, CollectionThroughputPropertyName);
            mapping.Add(SharedDocumentDbSinkAdapterConfigurationProperties.UseIndexingPolicyFile, UseIndexingPolicyFilePropertyName);
            mapping.Add(SharedDocumentDbSinkAdapterConfigurationProperties.IndexingPolicy, IndexingPolicyPropertyName);
            mapping.Add(SharedDocumentDbSinkAdapterConfigurationProperties.IndexingPolicyFile, IndexingPolicyFilePropertyName);
            mapping.Add(SharedDocumentDbSinkAdapterConfigurationProperties.IdField, IdFieldPropertyName);
            mapping.Add(SharedDocumentDbSinkAdapterConfigurationProperties.DisableIdGeneration, DisableIdGenerationPropertyName);
            mapping.Add(SharedDocumentDbSinkAdapterConfigurationProperties.UpdateExisting, UpdateExistingPropertyName);
            return mapping;
        }
    }
}
