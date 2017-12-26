using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.MongoDb.Shared;
using Microsoft.DataTransfer.WpfHost.Basics;

namespace Microsoft.DataTransfer.MongoDb.Wpf.Shared
{
    abstract class MongoDbAdapterConfiguration : ValidatableBindableBase, IMongoDbAdapterConfiguration
    {
        public static readonly string ConnectionStringPropertyName =
            ObjectExtensions.MemberName<IMongoDbAdapterConfiguration>(c => c.ConnectionString);

        public static readonly string CollectionPropertyName =
            ObjectExtensions.MemberName<IMongoDbAdapterConfiguration>(c => c.Collection);

        public static readonly string IsCosmosDBHostedPropertyName =
            ObjectExtensions.MemberName<IMongoDbAdapterConfiguration>(c => c.IsCosmosDBHosted);

        private string connectionString;
        private string collection;
        private bool isCosmosDBHosted;

        public string ConnectionString
        {
            get { return connectionString; }
            set { SetProperty(ref connectionString, value, ValidateNonEmptyString); }
        }

        public string Collection
        {
            get { return collection; }
            set { SetProperty(ref collection, value, ValidateNonEmptyString); }
        }

        public bool IsCosmosDBHosted
        {
            get { return isCosmosDBHosted; }
            set { SetProperty(ref isCosmosDBHosted, value); }
        }
    }
}
