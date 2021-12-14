using Microsoft.DataTransfer.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.DataTransfer.MongoDb.FunctionalTests
{
    [TestClass]
    public abstract class MongoDbAdapterTestBase : DataTransferAdapterTestBase
    {
        protected string ConnectionString { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            if (String.IsNullOrWhiteSpace(Settings.MongoConnectionString))
            {
                Assert.Inconclusive("You must provide a connection string value for the MongoConnectionString property in the Microsoft.DataTransfer.MongoDb.FunctionalTests/.runsettings file.");
            }

            string databaseName = $"Test{Guid.NewGuid():N};";
            ConnectionString = $"{Settings.MongoConnectionString}/{databaseName}";

            TestInitialize();
        }

        protected virtual void TestInitialize() { }

        [TestCleanup]
        public void Cleanup()
        {
            TestCleanup();
        }

        protected virtual void TestCleanup() { }
    }
}
