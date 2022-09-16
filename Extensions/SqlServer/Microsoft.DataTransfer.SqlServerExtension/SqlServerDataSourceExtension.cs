using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using Microsoft.DataTransfer.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Microsoft.DataTransfer.SqlServerExtension
{
    [Export(typeof(IDataSourceExtension))]
    public class SqlServerDataSourceExtension : IDataSourceExtension
    {
        public string DisplayName => "SqlServer";

        public IAsyncEnumerable<IDataItem> ReadAsync(IConfiguration config, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var settings = config.Get<SqlServerSourceSettings>();
            settings.Validate();

            throw new NotImplementedException();
        }
    }
}