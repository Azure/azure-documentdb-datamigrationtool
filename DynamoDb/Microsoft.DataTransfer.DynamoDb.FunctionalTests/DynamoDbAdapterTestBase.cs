using Microsoft.DataTransfer.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.DataTransfer.DynamoDb.FunctionalTests
{
    [TestClass]
    public abstract class DynamoDbAdapterTestBase : DataTransferAdapterTestBase
    {
        protected string ConnectionString { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            if (String.IsNullOrWhiteSpace(Settings.DynamoDbConnectionString))
            {
                Assert.Inconclusive("You must provide a connection string value for the DynamoDbConnectionString property in the Microsoft.DataTransfer.DynamoDb.FunctionalTests/.runsettings file.");
            }
            ConnectionString = Settings.DynamoDbConnectionString;

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
