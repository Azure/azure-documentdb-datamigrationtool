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
    public class NestedDocumentsTests : SqlTestsBase
    {
        private static string[] ColumnKeys = new[]
        {
            "IntColumn",
            "NVarCharMaxColumn",
        };

        private static Dictionary<string, string> ColumnMappings = new Dictionary<string, string>
        {
            { NestedDocumentsTests.ColumnKeys[0], "int primary key" },
            { NestedDocumentsTests.ColumnKeys[1], "nvarchar(max)" },
        };

        private string tableName;
        private string nestedTableName;
        private string anotherNestedTableName;

        private Dictionary<string, object>[] rows = new[]
            {
                new Dictionary<string, object>
                {
                    { NestedDocumentsTests.ColumnKeys[0], 1 },
                    { NestedDocumentsTests.ColumnKeys[1], "String1" },
                },
                new Dictionary<string, object>
                {
                    { NestedDocumentsTests.ColumnKeys[0], 2 },
                    { NestedDocumentsTests.ColumnKeys[1], "String2" },
                },
            };

        private Dictionary<string, object>[] nestedRows = new[]
            {
                new Dictionary<string, object>
                {
                    { NestedDocumentsTests.ColumnKeys[0], 1 },
                    { NestedDocumentsTests.ColumnKeys[1], "NestedString1" },
                },
                new Dictionary<string, object>
                {
                    { NestedDocumentsTests.ColumnKeys[0], 2 },
                    { NestedDocumentsTests.ColumnKeys[1], "NestedString2" },
                },
            };

        private Dictionary<string, object>[] anotherNestedRows = new[]
            {
                new Dictionary<string, object>
                {
                    { NestedDocumentsTests.ColumnKeys[0], 1 },
                    { NestedDocumentsTests.ColumnKeys[1], "AnotherNestedString1" },
                },
                new Dictionary<string, object>
                {
                    { NestedDocumentsTests.ColumnKeys[0], 2 },
                    { NestedDocumentsTests.ColumnKeys[1], "AnotherNestedString2" },
                },
            };

        [TestInitialize]
        public void Initialize()
        {
            tableName = CreateTableName();
            nestedTableName = CreateTableName();
            anotherNestedTableName = CreateTableName();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                CreateTable(connection, tableName, ColumnMappings);
                AddRows(connection, tableName, rows, false);

                CreateTable(connection, nestedTableName, ColumnMappings);
                AddRows(connection, nestedTableName, nestedRows, false);

                CreateTable(connection, anotherNestedTableName, ColumnMappings);
                AddRows(connection, anotherNestedTableName, anotherNestedRows, false);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                DropTable(connection, tableName);
                DropTable(connection, nestedTableName);
                DropTable(connection, anotherNestedTableName);
            }
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadOneLevelNestedDocuments_AllDataRead()
        {
            var configuration = 
                Mocks
                    .Of<ISqlDataSourceAdapterConfiguration>()
                    .Where(c => 
                        c.ConnectionString == ConnectionString &&
                        c.Query == String.Format(TestResources.OneLevelNestedSelectQueryFormat,
                            tableName, nestedTableName, anotherNestedTableName) &&
                        c.NestingSeparator == ".")
                    .First();

            var expectedTopLevelColumns = new[] { "IntColumn", "NVarCharMaxColumn", "Nested", "AnotherNested" };

            using (var adapter = await new SqlDataSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                for (var rowIndex = 0; rowIndex < rows.Length; ++rowIndex)
                {
                    var dataItem = await adapter.ReadNextAsync(ReadOutputByRef.None, CancellationToken.None);

                    Assert.IsNotNull(dataItem, TestResources.MoreDataItemsExpected);

                    CollectionAssert.AreEquivalent(expectedTopLevelColumns.ToArray(), dataItem.GetFieldNames().ToArray());

                    VerifyDataItem(rows[rowIndex], dataItem);

                    var nestedDataItem = dataItem.GetValue("Nested") as IDataItem;
                    Assert.IsNotNull(nestedDataItem, TestResources.InvalidPropertyValueFormat, "Nested");
                    VerifyDataItem(nestedRows[rowIndex], nestedDataItem);

                    var anotherNestedDataItem = dataItem.GetValue("AnotherNested") as IDataItem;
                    Assert.IsNotNull(anotherNestedDataItem, TestResources.InvalidPropertyValueFormat, "AnotherNested");
                    VerifyDataItem(anotherNestedRows[rowIndex], anotherNestedDataItem);
                }
            }
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadTwoLevelNestedDocuments_AllDataRead()
        {
            var configuration = 
                Mocks
                    .Of<ISqlDataSourceAdapterConfiguration>()
                    .Where(c => 
                        c.ConnectionString == ConnectionString &&
                        c.Query == String.Format(TestResources.TwoLevelNestedSelectQueryFormat,
                            tableName, nestedTableName, anotherNestedTableName) &&
                        c.NestingSeparator == ".")
                    .First();

            var expectedTopLevelColumns = new[] { "IntColumn", "NVarCharMaxColumn", "FirstNested" };

            using (var adapter = await new SqlDataSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                for (var rowIndex = 0; rowIndex < rows.Length; ++rowIndex)
                {
                    var dataItem = await adapter.ReadNextAsync(ReadOutputByRef.None, CancellationToken.None);

                    Assert.IsNotNull(dataItem, TestResources.MoreDataItemsExpected);

                    CollectionAssert.AreEquivalent(expectedTopLevelColumns.ToArray(), dataItem.GetFieldNames().ToArray());

                    VerifyDataItem(rows[rowIndex], dataItem);

                    var firstNestedDataItem = dataItem.GetValue("FirstNested") as IDataItem;
                    Assert.IsNotNull(firstNestedDataItem, TestResources.InvalidPropertyValueFormat, "FirstNested");
                    VerifyDataItem(nestedRows[rowIndex], firstNestedDataItem);

                    var secondNestedDataItem = firstNestedDataItem.GetValue("SecondNested") as IDataItem;
                    Assert.IsNotNull(secondNestedDataItem, TestResources.InvalidPropertyValueFormat, "SecondNested");
                    VerifyDataItem(anotherNestedRows[rowIndex], secondNestedDataItem);
                }
            }
        }
    }
}
