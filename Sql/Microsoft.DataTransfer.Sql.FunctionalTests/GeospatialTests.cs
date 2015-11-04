using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Sql.Source;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Spatial;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Sql.FunctionalTests
{
    [TestClass]
    public class GeospatialTests : SqlTestsBase
    {
        private string tableName;

        [TestInitialize]
        public void Initialize()
        {
            tableName = CreateTableName();
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadGeospatialDataUsingRawQuery_AllDataRead()
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

        private async Task TestSqlDataSourceAsync(ISqlDataSourceAdapterConfiguration configuration)
        {
            var columnKeys = new[]
            {
                "IntColumn",
                "GeographyColumn"
            };
            var columnMappings = new Dictionary<string, string>
            {
                { columnKeys[0], "int primary key" },
                { columnKeys[1], "geography" }
            };
            var rows = new[]
            {
                new Dictionary<string, object>
                {
                    { columnKeys[0], 1 },
                    { columnKeys[1], "geography::STGeomFromText('POINT(-122.16 43.656)', 4326)" }
                },
                new Dictionary<string, object>
                {
                    { columnKeys[0], 2 },
                    { columnKeys[1], "geography::STGeomFromText('LINESTRING(-122.360 47.656, -122.343 47.656 )', 4326)" }
                },
                new Dictionary<string, object>
                {
                    { columnKeys[0], 3 },
                    { columnKeys[1], "geography::STGeomFromText('POLYGON((-122.358 47.653 , -122.348 47.649, -122.348 47.658, -122.358 47.658, -122.358 47.653))', 4326)" }
                },
            };

            var expectedRows = new[]
            {
                new Dictionary<string, object>
                {
                    { columnKeys[0], 1 },
                    { columnKeys[1], SampleData.Geospatial.AsPoint(new GeographyPosition(43.656, -122.16)) }
                },
                new Dictionary<string, object>
                {
                    { columnKeys[0], 2 },
                    { columnKeys[1], SampleData.Geospatial.AsLineString(new[]
                        {
                            new GeographyPosition(47.656, -122.360),
                            new GeographyPosition(47.656, -122.343)
                        }) 
                    }
                },
                new Dictionary<string, object>
                {
                    { columnKeys[0], 3 },
                    { columnKeys[1], SampleData.Geospatial.AsPolygon(new[]
                        {
                            new GeographyPosition(47.653, -122.358),
                            new GeographyPosition(47.649, -122.348),
                            new GeographyPosition(47.658, -122.348),
                            new GeographyPosition(47.658, -122.358),
                            new GeographyPosition(47.653, -122.358)
                        })
                    }
                }
            };

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                try
                {
                    CreateTable(connection, tableName, columnMappings);
                    AddRows(connection, tableName, rows, true);

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

                            VerifyDataItem(expectedRows[rowIndex], dataItem);
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
