using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
using Microsoft.DataTransfer.Sql.Shared;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Sql.Source
{
    sealed class SqlQueryDataSourceAdapter : SqlDataAdapterBase<ISqlDataSourceAdapterInstanceConfiguration>
    {
        private static RetryPolicy retryPolicy = new RetryPolicy(
            new SqlDatabaseTransientErrorDetectionStrategy(), new Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3)));

        private SqlCommand command;
        private SqlDataReader dataReader;
        private long rowNumber;

        public SqlQueryDataSourceAdapter(ISqlDataSourceAdapterInstanceConfiguration configuration)
            : base(configuration) { }

        public async Task InitializeAsync()
        {
            await retryPolicy.ExecuteAsync(ExecuteCommand);
        }

        private async Task ExecuteCommand()
        {
            if (Connection.State != ConnectionState.Open)
                await Connection.OpenAsync();

            command = new SqlCommand(Configuration.Query, Connection);
            dataReader = await command.ExecuteReaderAsync();
        }

        public override async Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellationToken)
        {
            readOutput.DataItemId = String.Format(CultureInfo.InvariantCulture, Resources.DataItemIdFormat, ++rowNumber);

            while (!await dataReader.ReadAsync(cancellationToken))
                if (!await dataReader.NextResultAsync())
                    return null;

            var dataItem = NestedDataItem.Create(Configuration.NestingSeparator);

            for (var fieldIndex = 0; fieldIndex < dataReader.FieldCount; ++fieldIndex)
                dataItem.AddProperty(
                    dataReader.GetName(fieldIndex),
                    dataReader.GetValue(fieldIndex));

            return dataItem;
        }

        public override void Dispose()
        {
            TrashCan.Throw(ref dataReader);
            TrashCan.Throw(ref command);

            base.Dispose();
        }
    }
}
