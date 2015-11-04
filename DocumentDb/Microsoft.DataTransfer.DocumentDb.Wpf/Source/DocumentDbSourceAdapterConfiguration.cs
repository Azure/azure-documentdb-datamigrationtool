using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.DocumentDb.Source;
using Microsoft.DataTransfer.DocumentDb.Wpf.Shared;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Source
{
    sealed class DocumentDbSourceAdapterConfiguration : DocumentDbAdapterConfiguration<ISharedDocumentDbAdapterConfiguration>, IDocumentDbSourceAdapterConfiguration
    {
        public static readonly string CollectionPropertyName =
            ObjectExtensions.MemberName<IDocumentDbSourceAdapterConfiguration>(c => c.Collection);

        public static readonly string InternalFieldsPropertyName =
            ObjectExtensions.MemberName<IDocumentDbSourceAdapterConfiguration>(c => c.InternalFields);

        public static readonly string QueryPropertyName =
            ObjectExtensions.MemberName<IDocumentDbSourceAdapterConfiguration>(c => c.Query);

        public static readonly string QueryFilePropertyName =
            ObjectExtensions.MemberName<IDocumentDbSourceAdapterConfiguration>(c => c.QueryFile);

        private string collection;

        private bool internalFields;

        private bool useQueryFile;
        private string query;
        private string queryFile;

        public string Collection
        {
            get { return collection; }
            set { SetProperty(ref collection, value, ValidateNonEmptyString); }
        }

        public bool InternalFields
        {
            get { return internalFields; }
            set { SetProperty(ref internalFields, value); }
        }

        public bool UseQueryFile
        {
            get { return useQueryFile; }
            set { SetProperty(ref useQueryFile, value); }
        }

        public string Query
        {
            get { return query; }
            set { SetProperty(ref query, value); }
        }

        public string QueryFile
        {
            get { return queryFile; }
            set { SetProperty(ref queryFile, value); }
        }

        public DocumentDbSourceAdapterConfiguration(ISharedDocumentDbAdapterConfiguration sharedConfiguration)
            : base(sharedConfiguration) { }
    }
}
