using Microsoft.DataTransfer.DocumentDb.Sink.Parallel;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.FunctionalTests
{
    [TestClass]
    public class DocumentDbParallelSinkAdapterTests : DocumentDbSinkAdapterTestBase
    {
        [TestMethod, Timeout(300000)]
        public async Task WriteSampleData_AllDataStored()
        {
            const string CollectionName = "Data";
            const int NumberOfItems = 42;

            var configuration =
                Mocks
                    .Of<IDocumentDbParallelSinkAdapterConfiguration>(m =>
                        m.ConnectionString == ConnectionString &&
                        m.Collection == new[] { CollectionName } &&
                        m.ParallelRequests == 1 &&
                        m.Retries == 100)
                    .First();

            var sampleData = SampleData.GetSimpleDataItems(NumberOfItems);

            using (var adapter = await new DocumentDbParallelSinkAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance))
            {
                await WriteDataAsync(adapter, sampleData);
            }

            VerifyData(sampleData, DocumentDbHelper.ReadDocuments(ConnectionString, "Data"));
        }

        [TestMethod, Timeout(300000)]
        public async Task WriteSampleData_RandomPartitioningAcrossTwoCollections_AllDataStored()
        {
            const int NumberOfItems = 100;

            var configuration =
                Mocks
                    .Of<IDocumentDbParallelSinkAdapterConfiguration>(m =>
                        m.ConnectionString == ConnectionString &&
                        m.Collection == new[] { "Data[0-1]" } &&
                        m.ParallelRequests == 1 &&
                        m.Retries == 100)
                    .First();

            var sampleData = SampleData.GetSimpleDataItems(NumberOfItems);

            using (var adapter = await new DocumentDbParallelSinkAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance))
            {
                await WriteDataAsync(adapter, sampleData);
            }

            var firstCollection = DocumentDbHelper.ReadDocuments(ConnectionString, "Data0");
            Assert.IsTrue(firstCollection.Count() > 0, TestResources.DataIsNotPartitioned);

            var secondCollection = DocumentDbHelper.ReadDocuments(ConnectionString, "Data1");
            Assert.IsTrue(secondCollection.Count() > 0, TestResources.DataIsNotPartitioned);

            VerifyData(sampleData, firstCollection.Union(secondCollection));
        }

        [TestMethod, Timeout(300000)]
        public async Task WriteSampleData_HashPartitioningAcrossTwoCollections_AllDataStored()
        {
            const int NumberOfItems = 100;

            var configuration =
                Mocks
                    .Of<IDocumentDbParallelSinkAdapterConfiguration>(m =>
                        m.ConnectionString == ConnectionString &&
                        m.Collection == new[] { "Data0", "Data1" } &&
                        m.PartitionKey == "StringProperty" &&
                        m.ParallelRequests == 1 &&
                        m.Retries == 100)
                    .First();

            var sampleData = SampleData.GetSimpleDataItems(NumberOfItems);

            using (var adapter = await new DocumentDbParallelSinkAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance))
            {
                await WriteDataAsync(adapter, sampleData);
            }

            var firstCollection = DocumentDbHelper.ReadDocuments(ConnectionString, "Data0");
            Assert.IsTrue(firstCollection.Count() > 0, TestResources.DataIsNotPartitioned);

            var secondCollection = DocumentDbHelper.ReadDocuments(ConnectionString, "Data1");
            Assert.IsTrue(secondCollection.Count() > 0, TestResources.DataIsNotPartitioned);

            VerifyData(sampleData, firstCollection.Union(secondCollection));
        }
    }
}
