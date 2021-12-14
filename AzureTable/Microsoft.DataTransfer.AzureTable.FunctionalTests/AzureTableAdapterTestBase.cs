using Microsoft.DataTransfer.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.DataTransfer.AzureTable.FunctionalTests
{
    [TestClass]
    public abstract class AzureTableAdapterTestBase : DataTransferAdapterTestBase
    {
        protected string ConnectionString { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            if (String.IsNullOrWhiteSpace(Settings.AzureStorageConnectionString))
            {
                Assert.Inconclusive("You must provide a connection string value for the AzureStorageConnectionString property in the Microsoft.DataTransfer.AzureTable.FunctionalTests/.runsettings file.");
            }

            ConnectionString = Settings.AzureStorageConnectionString;

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
