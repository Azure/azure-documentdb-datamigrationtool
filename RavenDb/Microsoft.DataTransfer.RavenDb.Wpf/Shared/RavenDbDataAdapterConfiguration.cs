using Microsoft.DataTransfer.RavenDb.Shared;
using Microsoft.DataTransfer.WpfHost.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;

namespace Microsoft.DataTransfer.RavenDb.Wpf.Shared
{
    abstract class RavenDbDataAdapterConfiguration : ValidatableConfiguration, IRavenDbDataAdapterConfiguration
    {
        public static readonly string ConnectionStringPropertyName =
            ObjectExtensions.MemberName<IRavenDbDataAdapterConfiguration>(c => c.ConnectionString);

        private string connectionString;

        public string ConnectionString
        {
            get { return connectionString; }
            set { SetProperty(ref connectionString, value, ValidateNonEmptyString); }
        }
    }
}
