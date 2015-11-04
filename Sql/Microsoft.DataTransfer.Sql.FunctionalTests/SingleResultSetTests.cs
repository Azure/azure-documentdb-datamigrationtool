using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Sql.Source;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Sql.FunctionalTests
{
    [TestClass]
    public class SingleResultSetTests : SqlTestsBase
    {
        private string tableName;

        [TestInitialize]
        public void Initialize()
        {
            tableName = CreateTableName();
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadSimpleDataUsingRawQuery_AllDataRead()
        {
            var configuration = 
                Mocks
                    .Of<ISqlDataSourceAdapterConfiguration>()
                    .Where(c => 
                        c.ConnectionString == ConnectionString &&
                        c.Query == String.Format(TestResources.SimpleSelectQueryFormat, tableName))
                    .First();

            await TestSqlDataSourceAsync(configuration);
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadSimpleDataUsingQueryFile_AllDataRead()
        {
            string queryFileName = null;

            try
            {
                queryFileName = CreateQueryFile(tableName);

                var configuration = 
                    Mocks
                        .Of<ISqlDataSourceAdapterConfiguration>()
                        .Where(c =>
                            c.ConnectionString == ConnectionString &&
                            c.QueryFile == queryFileName)
                        .First();

                await TestSqlDataSourceAsync(configuration);
            }
            finally
            {
                if (!String.IsNullOrEmpty(queryFileName) && File.Exists(queryFileName))
                {
                    File.Delete(queryFileName);
                }
            }
        }        

        private async Task TestSqlDataSourceAsync(ISqlDataSourceAdapterConfiguration configuration)
        {
            var columnKeys = new[]
            {
                "IntColumn",
                "BitColumn",
                "NVarCharMaxColumn",
                "FloatColumn",
                "DateTimeColumn",
            };
            var columnMappings = new Dictionary<string, string>
            {
                { columnKeys[0], "int primary key" },
                { columnKeys[1], "bit" },
                { columnKeys[2], "nvarchar(max)" },
                { columnKeys[3], "float" },
                { columnKeys[4], "datetime" },
            };
            var rows = new[]
            {
                new Dictionary<string, object>
                {
                    { columnKeys[0], 1 },
                    { columnKeys[1], false },
                    { columnKeys[2], "String1" },
                    { columnKeys[3], 2.3 }, 
                    { columnKeys[4], GetSampleDateTime() }
                },
                new Dictionary<string, object>
                {
                    { columnKeys[0], 2 },
                    { columnKeys[1], true },
                    { columnKeys[2], "String2" },
                    { columnKeys[3], 4.5 }, 
                    { columnKeys[4], GetSampleDateTime() }
                },
            };

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                try
                {
                    CreateTable(connection, tableName, columnMappings);
                    AddRows(connection, tableName, rows, false);

                    using (var adapter = await new SqlDataSourceAdapterFactory()
                        .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
                    {
                        var readOutput = new ReadOutputByRef();
                        for (var rowIndex = 0; rowIndex < rows.Length; ++rowIndex)
                        {
                            var dataItem = await adapter.ReadNextAsync(readOutput, CancellationToken.None);

                            Assert.IsNotNull(dataItem, TestResources.MoreDataItemsExpected);

                            Assert.IsNotNull(readOutput.DataItemId, CommonTestResources.MissingDataItemId);
                            readOutput.Wipe();

                            VerifyDataItem(rows[rowIndex], dataItem);
                        }
                    }
                }
                finally
                {
                    DropTable(connection, tableName);
                }
            }
        }
    }
}
