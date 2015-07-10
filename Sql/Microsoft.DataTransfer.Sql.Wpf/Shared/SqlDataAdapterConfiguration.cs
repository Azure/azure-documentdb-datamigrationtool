using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.Sql.Shared;
using Microsoft.DataTransfer.WpfHost.Basics;

namespace Microsoft.DataTransfer.Sql.Wpf.Shared
{
    abstract class SqlDataAdapterConfiguration : ValidatableBindableBase, ISqlDataAdapterConfiguration
    {
        public static readonly string ConnectionStringPropertyName =
            ObjectExtensions.MemberName<ISqlDataAdapterConfiguration>(c => c.ConnectionString);

        private string connectionString;

        public string ConnectionString
        {
            get { return connectionString; }
            set { SetProperty(ref connectionString, value, ValidateNonEmptyString); }
        }
    }
}
