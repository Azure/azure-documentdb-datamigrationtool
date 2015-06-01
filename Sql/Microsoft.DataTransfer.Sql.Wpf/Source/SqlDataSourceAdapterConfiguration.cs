using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.Sql.Source;
using Microsoft.DataTransfer.Sql.Wpf.Shared;

namespace Microsoft.DataTransfer.Sql.Wpf.Source
{
    sealed class SqlDataSourceAdapterConfiguration : SqlDataAdapterConfiguration, ISqlDataSourceAdapterConfiguration
    {
        public static readonly string QueryPropertyName =
            ObjectExtensions.MemberName<ISqlDataSourceAdapterConfiguration>(c => c.Query);

        public static readonly string QueryFilePropertyName =
            ObjectExtensions.MemberName<ISqlDataSourceAdapterConfiguration>(c => c.QueryFile);

        public static readonly string NestingSeparatorPropertyName =
            ObjectExtensions.MemberName<ISqlDataSourceAdapterConfiguration>(c => c.NestingSeparator);

        private bool useQueryFile;
        private string query;
        private string queryFile;
        private string nestingSeparator;

        public bool UseQueryFile
        {
            get { return useQueryFile; }
            set 
            {
                SetProperty(ref useQueryFile, value);
                ValidateQuery();
            }
        }

        public string Query
        {
            get { return useQueryFile ? null : query; }
            set
            {
                SetProperty(ref query, value);
                ValidateQuery();
            }
        }

        public string QueryFile
        {
            get { return useQueryFile ? queryFile : null; }
            set
            {
                SetProperty(ref queryFile, value);
                ValidateQuery();
            }
        }

        public string NestingSeparator
        {
            get { return nestingSeparator; }
            set { SetProperty(ref nestingSeparator, value); }
        }

        private void ValidateQuery()
        {
            SetErrors(QueryPropertyName, !useQueryFile ? ValidateNonEmptyString(query) : null);
            SetErrors(QueryFilePropertyName, useQueryFile ? ValidateNonEmptyString(queryFile) : null);
        }
    }
}
