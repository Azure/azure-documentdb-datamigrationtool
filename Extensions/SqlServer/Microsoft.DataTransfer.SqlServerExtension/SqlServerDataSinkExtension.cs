using System.ComponentModel.Composition;
using Microsoft.DataTransfer.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Microsoft.DataTransfer.SqlServerExtension
{
    [Export(typeof(IDataSinkExtension))]
    public class SqlServerDataSinkExtension : IDataSinkExtension
    {
        public string DisplayName => "SqlServer";

        public async Task WriteAsync(IAsyncEnumerable<IDataItem> dataItems, IConfiguration config, CancellationToken cancellationToken = default)
        {
            var settings = config.Get<SqlServerSinkSettings>();
            settings.Validate();

            throw new NotImplementedException();

        }
    }
}