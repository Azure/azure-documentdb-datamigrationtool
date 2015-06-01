using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.RavenDb.Source;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.RavenDb.FunctionalTests
{
    [TestClass]
    public class RavenDbSourceAdapterComplexTests : DataTransferTestBase
    {
        private string connectionString;
        private Dictionary<string, object>[] sampleData;

        [TestInitialize]
        public void Initialize()
        {
            connectionString = Settings.RavenDbConnectionString(Guid.NewGuid().ToString("N"));
            sampleData = GetSampleData();
            RavenDbHelper.CreateSampleDatabase(connectionString, sampleData);
            RavenDbHelper.CreateIndex(connectionString, "AllDocs/ByAge", "from doc in docs select new { Age = doc.Age }");
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (!String.IsNullOrEmpty(connectionString))
                RavenDbHelper.DeleteDatabase(connectionString);
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadData_ApplyOver40Query_MatchingDocumentsRead()
        {
            var configuration =
                Mocks
                    .Of<IRavenDbSourceAdapterConfiguration>(c =>
                        c.ConnectionString == connectionString &&
                        c.Query == "Age: [40 TO *]" &&
                        c.Index == "AllDocs/ByAge" &&
                        c.ExcludeId == true)
                    .First();

            var readResults = new List<IDataItem>();
            using (var adapter = await new RavenDbSourceAdapterFactory()
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
                sampleData.Where(d => (int)d["Age"] >= 40), readResults, TestResources.InvalidDocumentsRead);
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadData_ApplyBelow40Query_MatchingDocumentsRead()
        {
            var configuration =
                Mocks
                    .Of<IRavenDbSourceAdapterConfiguration>(c =>
                        c.ConnectionString == connectionString &&
                        c.Query == "Age: [* TO 40]" &&
                        c.Index == "AllDocs/ByAge" &&
                        c.ExcludeId == true)
                    .First();

            var readResults = new List<IDataItem>();
            using (var adapter = await new RavenDbSourceAdapterFactory()
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
                sampleData.Where(d => (int)d["Age"] <= 40), readResults, TestResources.InvalidDocumentsRead);
        }

        private static Dictionary<string, object>[] GetSampleData()
        {
            return new Dictionary<string, object>[]
            {
                new Dictionary<string, object>
                {
                    { "FirstName", "John" },
                    { "LastName", "Smith" },
                    { "Age", 30 },
                    { "Manager", new Dictionary<string, object>
                        {
                            { "FirstName", "Adalia" },
                            { "LastName", "Oberon" }
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "FirstName", "Edison" },
                    { "LastName", "Cain" },
                    { "Age", 50 }
                },
            };
        }
    }
}
