using Microsoft.DataTransfer.DynamoDb.Source;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DynamoDb.FunctionalTests
{
    [TestClass]
    public class DynamoDbSourceAdapterComplexTests : DynamoDbSourceAdapterTestBase
    {
        [TestMethod, Timeout(120000)]
        [DeploymentItem(@"Requests\SimpleScanTemplate.json", @"DynamoDb")]
        public async Task Read_ScanComplexData_AllDataRead()
        {
            var configuration =
                Mocks
                    .Of<IDynamoDbSourceAdapterConfiguration>(c =>
                        c.ConnectionString == Settings.DynamoDbConnectionString &&
                        c.Request == File.ReadAllText(@"DynamoDb\SimpleScanTemplate.json").Replace("%TABLENAME%", TableName))
                    .First();

            DataItemCollectionAssert.AreEquivalent(Data,
                await ReadData(configuration), TestResources.InvalidDocumentsRead);
        }

        [TestMethod, Timeout(120000)]
        [DeploymentItem(@"Requests\SimpleQueryTemplate.json", @"DynamoDb")]
        public async Task Read_QueryComplexData_AllDataRead()
        {
            var configuration =
                Mocks
                    .Of<IDynamoDbSourceAdapterConfiguration>(c =>
                        c.ConnectionString == Settings.DynamoDbConnectionString &&
                        c.Request == File.ReadAllText(@"DynamoDb\SimpleQueryTemplate.json").Replace("%TABLENAME%", TableName))
                    .First();

            DataItemCollectionAssert.AreEquivalent(Data,
                await ReadData(configuration), TestResources.InvalidDocumentsRead);
        }

        protected override Dictionary<string, object>[] CreateSampleData()
        {
            return new[]
            {
                new Dictionary<string, object>
                {
                    { "HashKey", "test" },
                    { "RangeKey", 0 },
                    { "TopLevelProperty", "Hello world!" },
                    { "Nested", new Dictionary<string, object>
                        {
                            { "NestedPropertyA", "I am nested!" },
                            { "NestedPropertyB", 42 }
                        }
                    },
                    { "Collection", new object[]
                        {
                            "Value1",
                            "Value2",
                            new Dictionary<string, object>
                            {
                                { "CollectionObjectProperty", true }
                            },
                            100
                        }
                    },
                    { "Binary", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 } }
                }
            };
        }
    }
}
