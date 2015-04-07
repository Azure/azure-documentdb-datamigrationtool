using Microsoft.DataTransfer.DocumentDb.Source;
using Microsoft.DataTransfer.DocumentDb.Wpf.Shared;
using Microsoft.DataTransfer.WpfHost.Basics.Extensions;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Source
{
    sealed class DocumentDbSourceAdapterConfiguration : DocumentDbAdapterConfiguration, IDocumentDbSourceAdapterConfiguration
    {
        public static readonly string InternalFieldsPropertyName =
            ObjectExtensions.MemberName<IDocumentDbSourceAdapterConfiguration>(c => c.InternalFields);

        public static readonly string QueryPropertyName =
            ObjectExtensions.MemberName<IDocumentDbSourceAdapterConfiguration>(c => c.Query);

        public static readonly string QueryFilePropertyName =
            ObjectExtensions.MemberName<IDocumentDbSourceAdapterConfiguration>(c => c.QueryFile);

        private bool internalFields;
        private bool useQueryFile;
        private string query;
        private string queryFile;

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
    }
}
