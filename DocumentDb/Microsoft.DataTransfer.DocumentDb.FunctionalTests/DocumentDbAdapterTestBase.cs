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
            if (String.IsNullOrWhiteSpace(Settings.DocumentDbConnectionString))
            {
                Assert.Inconclusive("You must provide a connection string value for the DocumentDbConnectionString property in the Microsoft.DataTransfer.DocumentDB.FunctionalTests/.runsettings file.");
            }

            string databaseName = $"Test{Guid.NewGuid():N};";
            ConnectionString = $"{Settings.DocumentDbConnectionString}Database={databaseName};";

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
