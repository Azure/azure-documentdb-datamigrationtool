using Microsoft.DataTransfer.HBase.Source;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.HBase.FunctionalTests
{
    [TestClass]
    public class HBaseSourceAdapterSimpleTests : HBaseAdapterTestBase
    {
        private const int NumberOfItems = 20;

        private string tableName;
        private Dictionary<string, object>[] sampleData;

        protected override void TestInitialize()
        {
            tableName = Guid.NewGuid().ToString("N");
            sampleData = SampleData
                .GetSimpleDocuments(NumberOfItems)
                .Select(i => i
                    // Add RowId field
                    .Union(new[]
                        {
                            new KeyValuePair<string, object>(HBaseHelper.RowIdPropertyName, i["id"])
                        })
                    .ToDictionary(p => p.Key, p => p.Value))
                .ToArray();
            HBaseHelper.CreateSampleTable(ConnectionString, tableName, sampleData);
        }

        protected override void TestCleanup()
        {
            if (!String.IsNullOrEmpty(tableName))
                HBaseHelper.DeleteTable(ConnectionString, tableName);
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadSampleData_IncludeRowId_AllDataRead()
        {
            var configuration =
                Mocks
                    .Of<IHBaseSourceAdapterConfiguration>(c =>
                        c.ConnectionString == ConnectionString &&
                        c.Table == tableName)
                    .First();

            DataItemCollectionAssert.AreEquivalent(
                sampleData.Select(i => i
                    .ToDictionary(
                        p => p.Key == HBaseHelper.RowIdPropertyName ? p.Key : "data:" + p.Key,
                        p => (object)p.Value.ToString())),
                await ReadData(configuration),
                TestResources.InvalidDocumentsRead);
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadSampleData_ExcludeRowId_DataWithoutRowIdRead()
        {
            var configuration =
                Mocks
                    .Of<IHBaseSourceAdapterConfiguration>(c =>
                        c.ConnectionString == ConnectionString &&
                        c.Table == tableName &&
                        c.ExcludeId == true)
                    .First();

            DataItemCollectionAssert.AreEquivalent(
                sampleData.Select(i => i
                    .Where(p => p.Key != HBaseHelper.RowIdPropertyName)
                    .ToDictionary(
                        p => "data:" + p.Key,
                        p => (object)p.Value.ToString())),
                await ReadData(configuration),
                TestResources.InvalidDocumentsRead);
        }
    }
}