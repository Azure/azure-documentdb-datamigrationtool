using Microsoft.Azure.Documents;
using Microsoft.DataTransfer.DocumentDb.Exceptions;
using Microsoft.DataTransfer.DocumentDb.Sink.Bulk;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
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
    public class DocumentDbBulkSinkAdapterTests : DocumentDbSinkAdapterTestBase
    {
        [TestMethod, Timeout(300000)]
        [DeploymentItem("BulkInsert.js")]
        public async Task BulkWriteSampleData_AllDataStored()
        {
            const string CollectionName = "Data";
            const int NumberOfItems = 42;

            var configuration =
                Mocks
                    .Of<IDocumentDbBulkSinkAdapterConfiguration>(m =>
                        m.ConnectionString == ConnectionString &&
                        m.Collection == new[] { CollectionName } &&
                        m.BatchSize == 10 &&
                        m.MaxScriptSize == 1024)
                    .First();

            var sampleData = SampleData.GetSimpleDataItems(NumberOfItems);

            using (var adapter = await new DocumentDbBulkSinkAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                await WriteDataAsync(adapter, sampleData);
            }

            VerifyData(sampleData, DocumentDbHelper.ReadDocuments(ConnectionString, CollectionName));
        }

        [TestMethod, Timeout(300000)]
        [DeploymentItem("BulkInsert.js")]
        public async Task BulkWriteSampleData_RandomPartitioningAcrossTwoCollections_AllDataStored()
        {
            const int NumberOfItems = 100;

            var configuration =
                Mocks
                    .Of<IDocumentDbBulkSinkAdapterConfiguration>(m =>
                        m.ConnectionString == ConnectionString &&
                        m.Collection == new[] { "Data[0-1]" } &&
                        m.BatchSize == 10 &&
                        m.MaxScriptSize == 1024)
                    .First();

            var sampleData = SampleData.GetSimpleDataItems(NumberOfItems);

            using (var adapter = await new DocumentDbBulkSinkAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
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
        [DeploymentItem("BulkInsert.js")]
        public async Task BulkWriteSampleData_HashPartitioningAcrossTwoCollections_AllDataStored()
        {
            const int NumberOfItems = 100;

            var configuration =
                Mocks
                    .Of<IDocumentDbBulkSinkAdapterConfiguration>(m =>
                        m.ConnectionString == ConnectionString &&
                        m.Collection == new[] { "Data0", "Data1" } &&
                        m.PartitionKey == "StringProperty" &&
                        m.BatchSize == 10 &&
                        m.MaxScriptSize == 1024)
                    .First();

            var sampleData = SampleData.GetSimpleDataItems(NumberOfItems);

            using (var adapter = await new DocumentDbBulkSinkAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
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
        [DeploymentItem("BulkInsert.js")]
        public async Task BulkWriteSampleData_HashPartitioningAcrossTwoCollectionsOnNonStringField_AllDataStored()
        {
            const int NumberOfItems = 100;

            var configuration =
                Mocks
                    .Of<IDocumentDbBulkSinkAdapterConfiguration>(m =>
                        m.ConnectionString == ConnectionString &&
                        m.Collection == new[] { "Data0", "Data1" } &&
                        m.PartitionKey == "IntegerProperty" &&
                        m.BatchSize == 10 &&
                        m.MaxScriptSize == 1024)
                    .First();

            var sampleData = SampleData.GetSimpleDataItems(NumberOfItems);

            using (var adapter = await new DocumentDbBulkSinkAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
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
        [DeploymentItem("BulkInsert.js")]
        public async Task BulkWriteGeospatialData_AllDataStored()
        {
            const string CollectionName = "GeoData";

            var configuration =
                Mocks
                    .Of<IDocumentDbBulkSinkAdapterConfiguration>(m =>
                        m.ConnectionString == ConnectionString &&
                        m.Collection == new[] { CollectionName } &&
                        m.BatchSize == 10 &&
                        m.MaxScriptSize == 1024)
                    .First();

            var sampleData = GetSampleGeospatialDataItems();

            using (var adapter = await new DocumentDbBulkSinkAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                await WriteDataAsync(adapter, sampleData);
            }

            VerifyData(GetExpectedGeospatialDataItems(), DocumentDbHelper.ReadDocuments(ConnectionString, CollectionName));
        }

        [TestMethod, Timeout(300000)]
        [DeploymentItem("BulkInsert.js")]
        [ExpectedException(typeof(FailedToCreateDocumentException))]
        public async Task BulkWriteSampleData_CreateDuplicates_FailsToCreateDocumentWithSameId()
        {
            const string CollectionName = "DuplicatesData";

            var configuration =
                Mocks
                    .Of<IDocumentDbBulkSinkAdapterConfiguration>(m =>
                        m.ConnectionString == ConnectionString &&
                        m.Collection == new[] { CollectionName } &&
                        m.BatchSize == 10 &&
                        m.MaxScriptSize == 1024)
                    .First();

            var sampleData = GetSampleDuplicateDataItems();

            using (var adapter = await new DocumentDbBulkSinkAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                await WriteDataAsync(adapter, sampleData);
            }
        }

        [TestMethod, Timeout(300000)]
        [DeploymentItem("BulkInsert.js")]
        public async Task BulkWriteSampleData_UpsertDuplicates_AllDataStored()
        {
            const string CollectionName = "DupicatesData";

            var configuration =
                Mocks
                    .Of<IDocumentDbBulkSinkAdapterConfiguration>(m =>
                        m.ConnectionString == ConnectionString &&
                        m.Collection == new[] { CollectionName } &&
                        m.BatchSize == 10 &&
                        m.MaxScriptSize == 1024 &&
                        m.UpdateExisting == true)
                    .First();

            var sampleData = GetSampleDuplicateDataItems();

            using (var adapter = await new DocumentDbBulkSinkAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                await WriteDataAsync(adapter, sampleData);
            }

            VerifyData(GetExpectedDuplicateDataItems(), DocumentDbHelper.ReadDocuments(ConnectionString, CollectionName));
        }

        [TestMethod, Timeout(300000)]
        [DeploymentItem("BulkInsert.js")]
        [ExpectedException(typeof(DocumentClientException))]
        public async Task BulkWriteSampleData_PartitionedCollection_ThrowsException()
        {
            const string CollectionName = "Data";
            const int NumberOfItems = 42;

            var configuration =
                Mocks
                    .Of<IDocumentDbBulkSinkAdapterConfiguration>(m =>
                        m.ConnectionString == ConnectionString &&
                        m.Collection == new[] { CollectionName } &&
                        m.CollectionThroughput == 10100 && // 10000 RUs is the current threshold for single partition collections
                        m.BatchSize == 10 &&
                        m.MaxScriptSize == 1024)
                    .First();

            var sampleData = SampleData.GetSimpleDataItems(NumberOfItems);

            using (var adapter = await new DocumentDbBulkSinkAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                await WriteDataAsync(adapter, sampleData);
            }
        }
    }
}
