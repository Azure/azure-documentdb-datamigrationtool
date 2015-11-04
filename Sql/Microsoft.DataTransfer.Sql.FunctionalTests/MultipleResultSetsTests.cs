using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Sql.Source;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Sql.FunctionalTests
{
    [TestClass]
    public class MultipleResultSetsTests : SqlTestsBase
    {
        private static string[] ColumnKeys1 = new[]
        {
            "IntColumn",
            "NVarCharMaxColumn",
        };

        private static Dictionary<string, string> ColumnMappings1 = new Dictionary<string, string>
        {
            { ColumnKeys1[0], "int primary key" },
            { ColumnKeys1[1], "nvarchar(max)" },
        };

        private static string[] ColumnKeys3 = new[]
        {
            "IntColumn",
            "BitColumn",
            "FloatColumn",
        };

        private static Dictionary<string, string> ColumnMappings2 = new Dictionary<string, string>
        {
            { ColumnKeys3[0], "int primary key" },
            { ColumnKeys3[1], "bit" },
            { ColumnKeys3[2], "float" },
        };

        private string tableName1;
        private string emptyTableName1;
        private string tableName2;
        private string emptyTableName2;

        private Dictionary<string, object>[] rows1 = new[]
        {
            new Dictionary<string, object>
            {
                { ColumnKeys1[0], 1 },
                { ColumnKeys1[1], "String1" },
            },
            new Dictionary<string, object>
            {
                { ColumnKeys1[0], 2 },
                { ColumnKeys1[1], "String2" },
            },
        };

        private Dictionary<string, object>[] rows2 = new[]
        {
            new Dictionary<string, object>
            {
                { ColumnKeys3[0], 1 },
                { ColumnKeys3[1], false },
                { ColumnKeys3[2], 2.3 },
            },
            new Dictionary<string, object>
            {
                { ColumnKeys3[0], 2 },
                { ColumnKeys3[1], true },
                { ColumnKeys3[2], 4.5 },
            },
            new Dictionary<string, object>
            {
                { ColumnKeys3[0], 3 },
                { ColumnKeys3[1], true },
                { ColumnKeys3[2], 6.7 },
            },
        };

        [TestInitialize]
        public void Initialize()
        {
            tableName1 = CreateTableName();
            emptyTableName1 = CreateTableName();
            tableName2 = CreateTableName();
            emptyTableName2 = CreateTableName();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                CreateTable(connection, tableName1, ColumnMappings1);
                AddRows(connection, tableName1, rows1, false);

                CreateTable(connection, emptyTableName1, ColumnMappings1);

                CreateTable(connection, tableName2, ColumnMappings2);
                AddRows(connection, tableName2, rows2, false);

                CreateTable(connection, emptyTableName2, ColumnMappings2);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                DropTable(connection, tableName1);
                DropTable(connection, emptyTableName1);
                DropTable(connection, tableName2);
                DropTable(connection, emptyTableName2);
            }
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadUsingMultipleQueries_AllDataRead()
        {
            var configuration = 
                Mocks
                    .Of<ISqlDataSourceAdapterConfiguration>(c => 
                        c.ConnectionString == ConnectionString &&
                        c.Query == String.Join(Environment.NewLine, new[]
                            {
                                String.Format(TestResources.SimpleSelectQueryFormat, tableName1),
                                String.Format(TestResources.SimpleSelectQueryFormat, emptyTableName1),
                                String.Format(TestResources.SimpleSelectQueryFormat, tableName2),
                                String.Format(TestResources.SimpleSelectQueryFormat, emptyTableName2),
                            }) &&
                        c.NestingSeparator == ".")
                    .First();

            using (var adapter = await new SqlDataSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                await VerifyRows(adapter, rows1);
                await VerifyRows(adapter, rows2);

                var dataItem = await adapter.ReadNextAsync(new ReadOutputByRef(), CancellationToken.None);
                Assert.IsNull(dataItem, TestResources.UnexpectedDataItem);
            }
        }

        private async Task VerifyRows(IDataSourceAdapter adapter, IReadOnlyDictionary<string, object>[] expectedRows)
        {
            var readOutput = new ReadOutputByRef();

            for (var rowIndex = 0; rowIndex < expectedRows.Length; ++rowIndex)
            {
                var dataItem = await adapter.ReadNextAsync(readOutput, CancellationToken.None);

                Assert.IsNotNull(dataItem, TestResources.MoreDataItemsExpected);

                VerifyDataItem(expectedRows[rowIndex], dataItem);
            }
        }
    }
}
