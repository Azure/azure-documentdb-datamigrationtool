using Microsoft.DataTransfer.DocumentDb.Sink.Bulk;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.FunctionalTests
{
    [TestClass]
    public class DocumentDbBulkSinkAdapterTests : DocumentDbSinkAdapterTestBase
    {
        [TestMethod, Timeout(300000)]
        [DeploymentItem("BulkInsert.js")]
        public async Task WriteSampleData_AllDataStored()
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
                .CreateAsync(configuration, DataTransferContextMock.Instance))
            {
                await WriteDataAsync(adapter, sampleData);
            }

            VerifyData(sampleData, DocumentDbHelper.ReadDocuments(ConnectionString, "Data"));
        }

        [TestMethod, Timeout(300000)]
        [DeploymentItem("BulkInsert.js")]
        public async Task WriteSampleData_RandomPartitioningAcrossTwoCollections_AllDataStored()
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
        [DeploymentItem("BulkInsert.js")]
        public async Task WriteSampleData_HashPartitioningAcrossTwoCollections_AllDataStored()
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
        [DeploymentItem("BulkInsert.js")]
        public async Task WriteSampleData_HashPartitioningAcrossTwoCollectionsOnNonStringField_AllDataStored()
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
