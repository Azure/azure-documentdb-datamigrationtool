using Microsoft.DataTransfer.DocumentDb.Source;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.FunctionalTests
{
    [TestClass]
    public class DocumentDbSourceAdapterTests : DocumentDbAdapterTestBase
    {
        const string CollectionName = "Data";
        const int NumberOfItems = 200;

        private Dictionary<string, object>[] sampleData;

        protected override void TestInitialize()
        {
            sampleData = SampleData.GetSimpleDocuments(NumberOfItems);
            DocumentDbHelper
                .CreateSampleCollectionAsync(ConnectionString, CollectionName, sampleData)
                .Wait();
        }

        [TestMethod, Timeout(300000)]
        public async Task ReadSampleData_AllDataRead()
        {
            var configuration =
                Mocks
                    .Of<IDocumentDbSourceAdapterConfiguration>(c =>
                        c.ConnectionString == ConnectionString &&
                        c.Collection == CollectionName &&
                        c.InternalFields == false)
                    .First();

            List<IDataItem> readResults;
            using (var adapter = await new DocumentDbSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                readResults = await ReadDataAsync(adapter);
            }

            DataItemCollectionAssert.AreEquivalent(sampleData, readResults, TestResources.InvalidDocumentsRead);
        }
    }
}
