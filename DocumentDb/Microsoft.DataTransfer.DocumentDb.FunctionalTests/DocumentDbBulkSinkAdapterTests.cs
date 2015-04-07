using Microsoft.DataTransfer.DocumentDb.Sink.Bulk;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.FunctionalTests
{
    [TestClass]
    public class DocumentDbBulkSinkAdapterTests : DocumentDbAdapterTestBase
    {
        [TestMethod, Timeout(120000)]
        [DeploymentItem("BulkInsert.js")]
        public async Task WriteSampleData_AllDataStored()
        {
            const string CollectionName = "Data";
            const int NumberOfItems = 42;

            var configuration =
                Mocks
                    .Of<IDocumentDbBulkSinkAdapterConfiguration>(m =>
                        m.ConnectionString == ConnectionString &&
                        m.Collection == CollectionName &&
                        m.BatchSize == 10 &&
                        m.MaxScriptSize == 1024)
                    .First();

            using (var adapter = await new DocumentDbBulkSinkAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance))
            {
                await WriteDataAsync(adapter, SampleData.GetSimpleDataItems(NumberOfItems));
            }

            Assert.AreEqual(NumberOfItems, DocumentDbHelper.CountDocuments(ConnectionString, CollectionName),
                TestResources.InvalidNumberOfDocumentsWritten);
        }
    }
}
