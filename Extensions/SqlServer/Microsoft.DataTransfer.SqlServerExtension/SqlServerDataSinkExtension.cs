using System.ComponentModel.Composition;
using System.Data;
using Microsoft.Data.SqlClient;
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

            await using var connection = new SqlConnection(settings.ConnectionString);
            await connection.OpenAsync(cancellationToken);
            using var copy = new SqlBulkCopy(connection);
            // TODO: write data out in batches
            List<DataRow> dataRows = new List<DataRow>();
            await copy.WriteToServerAsync(dataRows.ToArray(), cancellationToken);
        }
    }
}