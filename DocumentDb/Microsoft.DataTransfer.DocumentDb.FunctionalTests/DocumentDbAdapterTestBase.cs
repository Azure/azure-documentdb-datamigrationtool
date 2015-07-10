using Microsoft.DataTransfer.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.DataTransfer.DocumentDb.FunctionalTests
{
    [TestClass]
    public abstract class DocumentDbAdapterTestBase : DataTransferAdapterTestBase
    {
        protected string ConnectionString { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            ConnectionString = Settings.DocumentDbConnectionString("Test" + Guid.NewGuid().ToString("N"));
            TestInitialize();
        }

        protected virtual void TestInitialize() { }

        [TestCleanup]
        public void Cleanup()
        {
            TestCleanup();
            if (ConnectionString != null)
                DocumentDbHelper.DeleteDatabaseAsync(ConnectionString).Wait();
        }

        protected virtual void TestCleanup() { }
    }
}
