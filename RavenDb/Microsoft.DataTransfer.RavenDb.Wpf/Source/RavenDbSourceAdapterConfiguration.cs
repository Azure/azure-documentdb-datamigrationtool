using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.RavenDb.Source;
using Microsoft.DataTransfer.RavenDb.Wpf.Shared;

namespace Microsoft.DataTransfer.RavenDb.Wpf.Source
{
    sealed class RavenDbSourceAdapterConfiguration : RavenDbAdapterConfiguration, IRavenDbSourceAdapterConfiguration
    {
        public static readonly string IndexPropertyName =
            ObjectExtensions.MemberName<IRavenDbSourceAdapterConfiguration>(c => c.Index);

        public static readonly string QueryPropertyName =
            ObjectExtensions.MemberName<IRavenDbSourceAdapterConfiguration>(c => c.Query);

        public static readonly string QueryFilePropertyName =
            ObjectExtensions.MemberName<IRavenDbSourceAdapterConfiguration>(c => c.QueryFile);

        public static readonly string ExcludeIdPropertyName =
            ObjectExtensions.MemberName<IRavenDbSourceAdapterConfiguration>(c => c.ExcludeId);

        private string index;

        private bool useQueryFile;
        private string query;
        private string queryFile;
        
        private bool excludeId;

        public string Index
        {
            get { return index; }
            set { SetProperty(ref index, value); }
        }

        public bool UseQueryFile
        {
            get { return useQueryFile; }
            set { SetProperty(ref useQueryFile, value); }
        }

        public string Query
        {
            get { return useQueryFile ? null : query; }
            set { SetProperty(ref query, value); }
        }

        public string QueryFile
        {
            get { return useQueryFile ? queryFile : null; }
            set { SetProperty(ref queryFile, value); }
        }

        public bool ExcludeId
        {
            get { return excludeId; }
            set { SetProperty(ref excludeId, value); }
        }
    }
}
