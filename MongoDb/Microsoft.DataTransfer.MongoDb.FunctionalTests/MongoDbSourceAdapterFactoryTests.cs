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
        public async Task CreateAsync_AmbiguousProjection_MongoQueryExceptionThrown()
        {
            var configuration =
                Mocks
                    .Of<IMongoDbSourceAdapterConfiguration>(c =>
                        c.ConnectionString == Settings.MongoConnectionString &&
                        c.Collection == CollectionName &&
                        c.Projection == "{regex: 0, script: 1}")
                    .First();

            try
            {
                using (await new MongoDbSourceAdapterFactory()
                    .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None)) { }

                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception error)
            {
                // Depending on which version of mongodb you are connecting to, you will get different exceptions.
                // Since we are trying to be as transparent with the user as possible in terms of error messages and
                // don't wrap them in the product code - we should expect either of them.
                Assert.IsTrue(
                    error is MongoQueryException || error is MongoCommandException,
                    "Invalid exception was thrown");
            }
        }
    }
}
