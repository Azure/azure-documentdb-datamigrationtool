using Microsoft.DataTransfer.RavenDb.Source;
using Microsoft.DataTransfer.TestsCommon;
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
    public class RavenDbSourceAdapterSimpleTests : RavenDbSourceAdapterTestBase
    {
        private const int NumberOfItems = 2000;

        private string connectionString;
        private Dictionary<string, object>[] sampleData;

        [TestInitialize]
        public void Initialize()
        {
            connectionString = Settings.RavenDbConnectionString(Guid.NewGuid().ToString("N"));
            sampleData = SampleData
                .GetSimpleDocuments(NumberOfItems)
                // Exclude DateTimeProperty since it is returned as a string from RavenDB and ruins the validation
                .Select(i => i
                    .Where(p => p.Key != "DateTimeProperty")
                    .ToDictionary(p => p.Key, p => p.Value))
                .ToArray();
            RavenDbHelper.CreateSampleDatabase(connectionString, sampleData);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (!String.IsNullOrEmpty(connectionString))
                RavenDbHelper.DeleteDatabase(connectionString);
        }

        [TestMethod, Timeout(120000)]
        public async Task ReadSampleData_AllDataRead()
        {
            var configuration =
                Mocks
                    .Of<IRavenDbSourceAdapterConfiguration>(c =>
                        c.ConnectionString == connectionString &&
                        c.ExcludeId == true)
                    .First();

            DataItemCollectionAssert.AreEquivalent(sampleData,
                await ReadData(configuration), TestResources.InvalidDocumentsRead);
        }
    }
}
