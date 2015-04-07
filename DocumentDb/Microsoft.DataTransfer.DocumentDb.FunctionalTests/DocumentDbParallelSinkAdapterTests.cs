using Microsoft.DataTransfer.DocumentDb.Sink.Parallel;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.FunctionalTests
{
    [TestClass]
    public class DocumentDbParallelSinkAdapterTests : DocumentDbAdapterTestBase
    {
        [TestMethod, Timeout(120000)]
        public async Task WriteSampleData_AllDataStored()
        {
            const string CollectionName = "Data";
            const int NumberOfItems = 42;

            var configuration =
                Mocks
                    .Of<IDocumentDbParallelSinkAdapterConfiguration>(m =>
                        m.ConnectionString == ConnectionString &&
                        m.Collection == CollectionName &&
                        m.ParallelRequests == 1 &&
                        m.Retries == 100)
                    .First();

            using (var adapter = await new DocumentDbParallelSinkAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance))
            {
                await WriteDataAsync(adapter, SampleData.GetSimpleDataItems(NumberOfItems));
            }

            Assert.AreEqual(NumberOfItems, DocumentDbHelper.CountDocuments(ConnectionString, CollectionName),
                TestResources.InvalidNumberOfDocumentsWritten);
        }
    }
}
