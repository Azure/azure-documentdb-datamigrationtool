using Microsoft.DataTransfer.DocumentDb.Sink.Parallel;
using Microsoft.DataTransfer.DocumentDb.Transformation;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.UnitTests.Sink
{
    [TestClass]
    public class DocumentDbParallelSinkAdapterTests : DataTransferAdapterTestBase
    {
        [TestMethod]
        public async Task WriteSampleData_Create_AllDataStored()
        {
            const string CollectionName = "TestCollection";
            const int NumberOfItems = 10;

            var clientMock = new DocumentDbWriteClientMock();
            
            var configuration = Mocks
                    .Of<IDocumentDbParallelSinkAdapterInstanceConfiguration>()
                    .Where(m => 
                        m.Collection == CollectionName &&
                        m.NumberOfParallelRequests == 2)
                    .First();

            using (var adapter = new DocumentDbParallelSinkAdapter(clientMock, PassThroughTransformation.Instance, configuration))
            {
                await adapter.InitializeAsync(CancellationToken.None);
                await WriteDataAsync(adapter, SampleData.GetSimpleDataItems(NumberOfItems));
            }

            CollectionAssert.AreEquivalent(new [] { CollectionName }, clientMock.CreatedCollections.ToArray(), TestResources.CollectionWasNotCreated);
            Assert.AreEqual(NumberOfItems, clientMock.NumberOfDocumentsCreated, TestResources.InvalidNumberOfDataItemsTransferred);
            Assert.AreEqual(0, clientMock.NumberOfDocumentsUpserted, TestResources.NoDocumentsShouldBeUpserted);
            Assert.AreEqual(0, clientMock.CreatedStoredProcedures.Count, TestResources.ParallelSinkAdapterCreatedStoredProcedure);
        }

        [TestMethod]
        public async Task WriteSampleData_Upsert_AllDataStored()
        {
            const string CollectionName = "TestCollection";
            const int NumberOfItems = 10;

            var clientMock = new DocumentDbWriteClientMock();

            var configuration = Mocks
                    .Of<IDocumentDbParallelSinkAdapterInstanceConfiguration>()
                    .Where(m =>
                        m.Collection == CollectionName &&
                        m.UpdateExisting == true &&
                        m.NumberOfParallelRequests == 2)
                    .First();

            using (var adapter = new DocumentDbParallelSinkAdapter(clientMock, PassThroughTransformation.Instance, configuration))
            {
                await adapter.InitializeAsync(CancellationToken.None);
                await WriteDataAsync(adapter, SampleData.GetSimpleDataItems(NumberOfItems));
            }

            CollectionAssert.AreEquivalent(new[] { CollectionName }, clientMock.CreatedCollections.ToArray(), TestResources.CollectionWasNotCreated);
            Assert.AreEqual(0, clientMock.NumberOfDocumentsCreated, TestResources.AllDocumentShouldBeUpserted);
            Assert.AreEqual(NumberOfItems, clientMock.NumberOfDocumentsUpserted, TestResources.InvalidNumberOfDataItemsTransferred);
            Assert.AreEqual(0, clientMock.CreatedStoredProcedures.Count, TestResources.ParallelSinkAdapterCreatedStoredProcedure);
        }
    }
}
