using Microsoft.Azure.Documents;
using Microsoft.DataTransfer.DocumentDb.Sink.Parallel;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.FunctionalTests
{
    [TestClass]
    [DeploymentItem("DocumentDB.Spatial.Sql.dll")]
    [DeploymentItem("Microsoft.Azure.Documents.ServiceInterop.dll")]
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
                        m.Collection == CollectionName &&
                        m.ParallelRequests == 1 &&
                        m.Retries == 100)
                    .First();

            var sampleData = SampleData.GetSimpleDataItems(NumberOfItems);

            using (var adapter = await new DocumentDbParallelSinkAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                await WriteDataAsync(adapter, sampleData);
            }

            VerifyData(sampleData, DocumentDbHelper.ReadDocuments(ConnectionString, CollectionName));
        }

        [TestMethod, Timeout(300000)]
        [DeploymentItem(@"IndexingPolicies\IntegerPropertyRangeIndex.json", "IndexingPolicies")]
        public async Task WriteSampleData_RangeIndexOnIntegerProperty_IntegerRangeFilterCanBeUsed()
        {
            const string CollectionName = "Data";
            const int NumberOfItems = 42;

            var configuration =
                Mocks
                    .Of<IDocumentDbParallelSinkAdapterConfiguration>(m =>
                        m.ConnectionString == ConnectionString &&
                        m.Collection == CollectionName &&
                        m.IndexingPolicyFile == @"IndexingPolicies\IntegerPropertyRangeIndex.json" &&
                        m.ParallelRequests == 1 &&
                        m.Retries == 100)
                    .First();

            var sampleData = SampleData.GetSimpleDataItems(NumberOfItems);

            using (var adapter = await new DocumentDbParallelSinkAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                await WriteDataAsync(adapter, sampleData);
            }

            VerifyData(
                sampleData.Where(i => (int)i.GetValue("IntegerProperty") < 20).ToArray(),
                DocumentDbHelper.ReadDocuments(ConnectionString, CollectionName, "SELECT * FROM c WHERE c.IntegerProperty < 20"));
        }

        [TestMethod, Timeout(300000)]
        public async Task WriteSampleData_HashPartitioning_AllDataStored()
        {
            const string CollectionName = "ElasticData";
            const int NumberOfItems = 100;

            var configuration =
                Mocks
                    .Of<IDocumentDbParallelSinkAdapterConfiguration>(m =>
                        m.ConnectionString == ConnectionString &&
                        m.Collection == CollectionName &&
                        m.CollectionThroughput == 20000 && // 20k should be equivalent to 2 partitions
                        m.PartitionKey == "/StringProperty" &&
                        m.ParallelRequests == 1 &&
                        m.Retries == 100)
                    .First();

            var sampleData = SampleData.GetSimpleDataItems(NumberOfItems);

            using (var adapter = await new DocumentDbParallelSinkAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                await WriteDataAsync(adapter, sampleData);
            }

            VerifyData(sampleData, DocumentDbHelper.ReadDocuments(ConnectionString, CollectionName));
        }

        [TestMethod, Timeout(300000)]
        public async Task WriteGeospatialData_AllDataStored()
        {
            const string CollectionName = "Data";

            var configuration =
                Mocks
                    .Of<IDocumentDbParallelSinkAdapterConfiguration>(m =>
                        m.ConnectionString == ConnectionString &&
                        m.Collection == CollectionName &&
                        m.ParallelRequests == 1 &&
                        m.Retries == 100)
                    .First();

            var sampleData = GetSampleGeospatialDataItems();

            using (var adapter = await new DocumentDbParallelSinkAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                await WriteDataAsync(adapter, sampleData);
            }

            VerifyData(GetExpectedGeospatialDataItems(), DocumentDbHelper.ReadDocuments(ConnectionString, CollectionName));
        }

        [TestMethod, Timeout(300000)]
        // Throws ConflictException, but since it's private - expect base class
        [ExpectedException(typeof(DocumentClientException), AllowDerivedTypes = true)]
        public async Task WriteSampleData_CreateDuplicates_FailsToCreateDocumentWithSameId()
        {
            const string CollectionName = "DuplicatesData";

            var configuration =
                Mocks
                    .Of<IDocumentDbParallelSinkAdapterConfiguration>(m =>
                        m.ConnectionString == ConnectionString &&
                        m.Collection == CollectionName &&
                        m.ParallelRequests == 1 &&
                        m.Retries == 100)
                    .First();

            var sampleData = GetSampleDuplicateDataItems();

            using (var adapter = await new DocumentDbParallelSinkAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                await WriteDataAsync(adapter, sampleData);
            }
        }

        [TestMethod, Timeout(300000)]
        public async Task WriteSampleData_CreateDuplicates_AllDataStored()
        {
            const string CollectionName = "DuplicatesData";

            var configuration =
                Mocks
                    .Of<IDocumentDbParallelSinkAdapterConfiguration>(m =>
                        m.ConnectionString == ConnectionString &&
                        m.Collection == CollectionName &&
                        m.ParallelRequests == 1 &&
                        m.Retries == 100 &&
                        m.UpdateExisting == true)
                    .First();

            var sampleData = GetSampleDuplicateDataItems();

            using (var adapter = await new DocumentDbParallelSinkAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                await WriteDataAsync(adapter, sampleData);
            }

            VerifyData(GetExpectedDuplicateDataItems(), DocumentDbHelper.ReadDocuments(ConnectionString, CollectionName));
        }
    }
}
