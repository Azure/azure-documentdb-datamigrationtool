using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.MongoDb.Source.Online;
using Microsoft.DataTransfer.MongoDb.Wpf.Shared;

namespace Microsoft.DataTransfer.MongoDb.Wpf.Source.Online
{
    sealed class MongoDbSourceAdapterConfiguration : MongoDbAdapterConfiguration, IMongoDbSourceAdapterConfiguration
    {
        public static readonly string QueryPropertyName = 
            ObjectExtensions.MemberName<IMongoDbSourceAdapterConfiguration>(c => c.Query);

        public static readonly string QueryFilePropertyName = 
            ObjectExtensions.MemberName<IMongoDbSourceAdapterConfiguration>(c => c.QueryFile);

        public static readonly string ProjectionPropertyName = 
            ObjectExtensions.MemberName<IMongoDbSourceAdapterConfiguration>(c => c.Projection);

        public static readonly string ProjectionFilePropertyName = 
            ObjectExtensions.MemberName<IMongoDbSourceAdapterConfiguration>(c => c.ProjectionFile);

        private bool useQueryFile;
        private string query;
        private string queryFile;

        private bool useProjectionFile;
        private string projection;
        private string projectionFile;

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

        public bool UseProjectionFile
        {
            get { return useProjectionFile; }
            set { SetProperty(ref useProjectionFile, value); }
        }

        public string Projection
        {
            get { return useProjectionFile ? null : projection; }
            set { SetProperty(ref projection, value); }
        }

        public string ProjectionFile
        {
            get { return useProjectionFile ? projectionFile : null; }
            set { SetProperty(ref projectionFile, value); }
        }
    }
}
