using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.DynamoDb.Shared;
using Microsoft.DataTransfer.WpfHost.Basics;

namespace Microsoft.DataTransfer.DynamoDb.Wpf.Shared
{
    abstract class DynamoDbAdapterConfiguration : ValidatableBindableBase, IDynamoDbAdapterConfiguration
    {
        public static readonly string ConnectionStringPropertyName =
            ObjectExtensions.MemberName<IDynamoDbAdapterConfiguration>(c => c.ConnectionString);

        private string connectionString;

        public string ConnectionString
        {
            get { return connectionString; }
            set { SetProperty(ref connectionString, value, ValidateNonEmptyString); }
        }
    }
}
