using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.HBase.Shared;
using Microsoft.DataTransfer.WpfHost.Basics;

namespace Microsoft.DataTransfer.HBase.Wpf.Shared
{
    abstract class HBaseAdapterConfiguration : ValidatableBindableBase, IHBaseAdapterConfiguration
    {
        public static readonly string ConnectionStringPropertyName =
            ObjectExtensions.MemberName<IHBaseAdapterConfiguration>(c => c.ConnectionString);

        private string connectionString;

        public string ConnectionString
        {
            get { return connectionString; }
            set { SetProperty(ref connectionString, value, ValidateNonEmptyString); }
        }
    }
}
