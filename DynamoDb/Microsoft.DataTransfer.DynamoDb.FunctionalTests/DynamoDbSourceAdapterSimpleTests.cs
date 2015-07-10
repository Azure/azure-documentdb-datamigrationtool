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
    public class DynamoDbSourceAdapterSimpleTests : DynamoDbSourceAdapterTestBase
    {
        [TestMethod, Timeout(120000)]
        [DeploymentItem(@"Requests\SimpleScanTemplate.json", @"DynamoDb")]
        public async Task Read_ScanSampleData_AllDataRead()
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
        [DeploymentItem(@"Requests\FilteredScanTemplate.json", @"DynamoDb")]
        public async Task Read_ScanSampleDataForIntegerOver50_FilteredDataRead()
        {
            var configuration =
                Mocks
                    .Of<IDynamoDbSourceAdapterConfiguration>(c =>
                        c.ConnectionString == Settings.DynamoDbConnectionString &&
                        c.Request == File.ReadAllText(@"DynamoDb\FilteredScanTemplate.json").Replace("%TABLENAME%", TableName))
                    .First();

            DataItemCollectionAssert.AreEquivalent(Data.Where(d => (int)d["IntegerProperty"] > 50),
                await ReadData(configuration), TestResources.InvalidDocumentsRead);
        }

        [TestMethod, Timeout(120000)]
        [DeploymentItem(@"Requests\SimpleQueryTemplate.json", @"DynamoDb")]
        public async Task Read_QuerySampleData_AllDataRead()
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

        [TestMethod, Timeout(120000)]
        [DeploymentItem(@"Requests\FilteredQueryTemplate.json", @"DynamoDb")]
        public async Task Read_QuerySampleDataForIntegerOver50_FilteredDataRead()
        {
            var configuration =
                Mocks
                    .Of<IDynamoDbSourceAdapterConfiguration>(c =>
                        c.ConnectionString == Settings.DynamoDbConnectionString &&
                        c.Request == File.ReadAllText(@"DynamoDb\FilteredQueryTemplate.json").Replace("%TABLENAME%", TableName))
                    .First();

            DataItemCollectionAssert.AreEquivalent(Data.Where(d => (int)d["IntegerProperty"] > 50),
                await ReadData(configuration), TestResources.InvalidDocumentsRead);
        }

        [TestMethod, Timeout(120000)]
        [DeploymentItem(@"Requests\NonExistingHashQueryTemplate.json", @"DynamoDb")]
        public async Task Read_QuerySampleDataWithNonExistingHashKey_NoDataRead()
        {
            var configuration =
                Mocks
                    .Of<IDynamoDbSourceAdapterConfiguration>(c =>
                        c.ConnectionString == Settings.DynamoDbConnectionString &&
                        c.Request == File.ReadAllText(@"DynamoDb\NonExistingHashQueryTemplate.json").Replace("%TABLENAME%", TableName))
                    .First();

            Assert.AreEqual(0, (await ReadData(configuration)).Count, TestResources.InvalidDocumentsRead);
        }

        protected override Dictionary<string, object>[] CreateSampleData()
        {
            return SampleData
                .GetSimpleDocuments(100)
                .Select(i => i
                    // Exclude DateTimeProperty since it is returned as a string from DynamoDB and ruins the validation
                    .Where(p => p.Key != "DateTimeProperty")
                    // Append HashKey property to use as a hashkey for the table
                    .Union(new[]
                        {
                            new KeyValuePair<string, object>("HashKey", "test"),
                            new KeyValuePair<string, object>("RangeKey", i["IntegerProperty"])
                        })
                    .ToDictionary(p => p.Key, p => p.Value))
                .ToArray();
        }
    }
}
