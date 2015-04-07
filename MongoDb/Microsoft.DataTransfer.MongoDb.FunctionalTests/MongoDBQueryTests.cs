using System;
using System.IO;
using System.Linq;
using Microsoft.DataTransfer.MongoDB.Source;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.DataTransfer.MongoDB.FunctionalTests
{
    [TestClass]
    public class QueryTests : DataTransferTestBase
    {
        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void QueryAndQueryFileSet()
        {
            var factory = new MongoDbSourceAdapterFactory();
            var configuration = Mocks.Of<IMongoDbSourceAdapterConfiguration>()
                .Where(c => c.ConnectionString == Settings.MongoConnectionString &&
                            c.Collection == "collect" && c.Query == "blah" && c.QueryFile == "testQueryFile.txt")
                .First();
            factory.Create(configuration);
        }

        [TestMethod]
        [ExpectedException(typeof (FileNotFoundException))]
        public void NonexistentQueryFile()
        {
            var factory = new MongoDbSourceAdapterFactory();
            var configuration = Mocks.Of<IMongoDbSourceAdapterConfiguration>()
                .Where(c => c.ConnectionString == Settings.MongoConnectionString &&
                            c.Collection == "collect" && c.QueryFile == "nonexistent").First();
            factory.Create(configuration);
        }

        [TestMethod]
        public void ValidQueryFile()
        {
            var factory = new MongoDbSourceAdapterFactory();
            string filename = "testQueryFile" + Guid.NewGuid() + ".txt";
            try
            {
                File.WriteAllText(filename, "{type: \"snacks\"}");
                var configuration = Mocks.Of<IMongoDbSourceAdapterConfiguration>()
                    .Where(c => c.ConnectionString == Settings.MongoConnectionString &&
                                c.Collection == "collect" && c.QueryFile == filename).First();
                factory.Create(configuration);
            }
            finally
            {
                if (File.Exists(filename)) File.Delete(filename);
            }
        }
    }
}