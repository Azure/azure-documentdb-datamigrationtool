using Microsoft.DataTransfer.MongoDb.Source.Online;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.MongoDb.FunctionalTests
{
    [TestClass]
    public class MongoDbSourceAdapterFactoryTests : DataTransferTestBase
    {
        private const string CollectionName = "TestCollection";

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CreateAsync_QueryAndQueryFileBothSet_ArgumentExceptionThrown()
        {
            var configuration =
                Mocks
                    .Of<IMongoDbSourceAdapterConfiguration>(c =>
                        c.ConnectionString == Settings.MongoConnectionString &&
                        c.Collection == CollectionName && 
                        c.Query == "blah" && 
                        c.QueryFile == "testQueryFile.txt")
                    .First();

            using (await new MongoDbSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None)) { }
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public async Task CreateAsync_QueryFileDoesNotExist_FileNotFoundExceptionThrown()
        {
            var configuration =
                Mocks
                    .Of<IMongoDbSourceAdapterConfiguration>(c =>
                        c.ConnectionString == Settings.MongoConnectionString &&
                        c.Collection == CollectionName &&
                        c.QueryFile == "nonexisting")
                    .First();

            using (await new MongoDbSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None)) { }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CreateAsync_ProjectionAndProjectionFileBothSet_ArgumentExceptionThrown()
        {
            var configuration =
                Mocks
                    .Of<IMongoDbSourceAdapterConfiguration>(c =>
                        c.ConnectionString == Settings.MongoConnectionString &&
                        c.Collection == CollectionName &&
                        c.Projection == "blah" &&
                        c.ProjectionFile == "testProjectionFile.txt")
                    .First();

            using (await new MongoDbSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None)) { }
        }

        [TestMethod]
        [ExpectedException(typeof(MongoQueryException))]
        public async Task CreateAsync_AmbiguousProjection_MongoQueryExceptionThrown()
        {
            var configuration =
                Mocks
                    .Of<IMongoDbSourceAdapterConfiguration>(c =>
                        c.ConnectionString == Settings.MongoConnectionString &&
                        c.Collection == CollectionName &&
                        c.Projection == "{regex: 0, script: 1}")
                    .First();

            using (await new MongoDbSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None)) { }
        }
    }
}
