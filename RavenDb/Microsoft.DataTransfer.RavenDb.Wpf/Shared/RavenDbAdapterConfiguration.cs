using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.RavenDb.Shared;
using Microsoft.DataTransfer.WpfHost.Basics;

namespace Microsoft.DataTransfer.RavenDb.Wpf.Shared
{
    abstract class RavenDbAdapterConfiguration : ValidatableBindableBase, IRavenDbAdapterConfiguration
    {
        public static readonly string ConnectionStringPropertyName =
            ObjectExtensions.MemberName<IRavenDbAdapterConfiguration>(c => c.ConnectionString);

        private string connectionString;

        public string ConnectionString
        {
            get { return connectionString; }
            set { SetProperty(ref connectionString, value, ValidateNonEmptyString); }
        }
    }
}
