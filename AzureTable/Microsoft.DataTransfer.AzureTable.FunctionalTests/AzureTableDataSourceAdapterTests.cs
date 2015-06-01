using Microsoft.DataTransfer.AzureTable.Source;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.AzureTable.FunctionalTests
{
    [TestClass]
    public class AzureTableDataSourceAdapterTests : DataTransferTestBase
    {
        private const int NumberOfItems = 2000;

        private string tableName;
        private Dictionary<string, object>[] sampleData;

        [TestInitialize]
        public void TestInitialize()
        {
            tableName = "Test" + Guid.NewGuid().ToString("N");
            sampleData = SampleData.GetSimpleDocuments(NumberOfItems);
            AzureTableHelper
                .CreateTable(Settings.AzureStorageConnectionString, tableName, sampleData);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (!String.IsNullOrEmpty(tableName))
                AzureTableHelper.DeleteTable(Settings.AzureStorageConnectionString, tableName);
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadAllEntities_AllDataRead()
        {
            var configuration = Mocks
                    .Of<IAzureTableSourceAdapterConfiguration>(c =>
                        c.ConnectionString == Settings.AzureStorageConnectionString &&
                        c.Table == tableName &&
                        c.InternalFields == AzureTableInternalFields.None)
                    .First();

            var readResults = new List<IDataItem>();
            using (var adapter = await new AzureTableSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance))
            {
                IDataItem dataItem;
                var readOutput = new ReadOutputByRef();
                while ((dataItem = await adapter.ReadNextAsync(readOutput, CancellationToken.None)) != null)
                {
                    readResults.Add(dataItem);

                    Assert.IsNotNull(readOutput.DataItemId, CommonTestResources.MissingDataItemId);
                    readOutput.Wipe();
                }
            }

            DataItemCollectionAssert.AreEquivalent(sampleData, readResults, TestResources.InvalidDocumentsRead);
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadEntitiesWithFilter_First100RecordsRead()
        {
            const string IntegerPropertyName = "IntegerProperty";

            var configuration = Mocks
                    .Of<IAzureTableSourceAdapterConfiguration>(c =>
                        c.ConnectionString == Settings.AzureStorageConnectionString &&
                        c.Table == tableName &&
                        c.Filter == IntegerPropertyName + " lt 100" &&
                        c.InternalFields == AzureTableInternalFields.None)
                    .First();

            var readResults = new List<IDataItem>();
            using (var adapter = await new AzureTableSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance))
            {
                IDataItem dataItem;
                var readOutput = new ReadOutputByRef();
                while ((dataItem = await adapter.ReadNextAsync(readOutput, CancellationToken.None)) != null)
                {
                    readResults.Add(dataItem);

                    Assert.IsNotNull(readOutput.DataItemId, CommonTestResources.MissingDataItemId);
                    readOutput.Wipe();
                }
            }

            DataItemCollectionAssert.AreEquivalent(sampleData.Where(e => (int)e[IntegerPropertyName] < 100), readResults, TestResources.InvalidDocumentsRead);
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadEntitiesWithProjection_OnlyStringPropertyRead()
        {
            const string StringPropertyName = "StringProperty";

            var configuration = Mocks
                    .Of<IAzureTableSourceAdapterConfiguration>(c =>
                        c.ConnectionString == Settings.AzureStorageConnectionString &&
                        c.Table == tableName &&
                        c.Projection == new[] { StringPropertyName } &&
                        c.InternalFields == AzureTableInternalFields.None)
                    .First();

            var readResults = new List<IDataItem>();
            using (var adapter = await new AzureTableSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance))
            {
                IDataItem dataItem;
                var readOutput = new ReadOutputByRef();
                while ((dataItem = await adapter.ReadNextAsync(readOutput, CancellationToken.None)) != null)
                {
                    readResults.Add(dataItem);

                    Assert.IsNotNull(readOutput.DataItemId, CommonTestResources.MissingDataItemId);
                    readOutput.Wipe();
                }
            }

            DataItemCollectionAssert.AreEquivalent(
                sampleData
                    .Select(i => new Dictionary<string, object> { { StringPropertyName, i[StringPropertyName] } })
                    .ToArray(),
                readResults,
                TestResources.InvalidDocumentsRead);
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadEntitiesWithRowKey_RowKeyPropertyRead()
        {
            var configuration = Mocks
                    .Of<IAzureTableSourceAdapterConfiguration>(c =>
                        c.ConnectionString == Settings.AzureStorageConnectionString &&
                        c.Table == tableName &&
                        c.InternalFields == AzureTableInternalFields.RowKey)
                    .First();

            await ReadAndVerifyFields(configuration, new[] { "RowKey" });
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadEntitiesWithAllInternalFields_AllInternalPropertiesRead()
        {
            var configuration = Mocks
                    .Of<IAzureTableSourceAdapterConfiguration>(c =>
                        c.ConnectionString == Settings.AzureStorageConnectionString &&
                        c.Table == tableName &&
                        c.InternalFields == AzureTableInternalFields.All)
                    .First();

            await ReadAndVerifyFields(configuration, new[] { "RowKey", "PartitionKey", "Timestamp" });
        }

        private static async Task ReadAndVerifyFields(IAzureTableSourceAdapterConfiguration configuration, string[] expectedInternalProperties)
        {
            using (var adapter = await new AzureTableSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance))
            {
                IDataItem dataItem;
                var readOutput = new ReadOutputByRef();
                while ((dataItem = await adapter.ReadNextAsync(readOutput, CancellationToken.None)) != null)
                {
                    var fieldNames = dataItem.GetFieldNames().ToArray();

                    foreach (var expectedInternalProperty in expectedInternalProperties)
                    {
                        CollectionAssert.Contains(fieldNames, expectedInternalProperty, TestResources.MissingDataItemFieldFormat, expectedInternalProperty);
                        Assert.IsNotNull(dataItem.GetValue(expectedInternalProperty), TestResources.EmptyDataItemFieldValueFormat, expectedInternalProperty);
                    }

                    Assert.IsNotNull(readOutput.DataItemId, CommonTestResources.MissingDataItemId);
                    readOutput.Wipe();
                }
            }
        }
    }
}
