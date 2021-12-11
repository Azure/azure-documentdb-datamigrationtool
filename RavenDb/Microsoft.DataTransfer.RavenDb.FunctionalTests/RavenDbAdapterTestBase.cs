using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.RavenDb.Source;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.RavenDb.FunctionalTests
{
    [TestClass]
    public abstract class RavenDbAdapterTestBase : DataTransferAdapterTestBase
    {
        protected string ConnectionString { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            if (String.IsNullOrWhiteSpace(Settings.RavenDbConnectionString))
            {
                Assert.Inconclusive("You must provide a connection string value for the RavenDbConnectionString property in the Microsoft.DataTransfer.RavenDb.FunctionalTests/.runsettings file.");
            }

            string databaseName = $"Test{Guid.NewGuid():N};";
            ConnectionString = $"{Settings.RavenDbConnectionString}Database={databaseName};";

            TestInitialize();
        }

        protected virtual void TestInitialize() { }

        [TestCleanup]
        public void Cleanup()
        {
            TestCleanup();
        }

        protected virtual void TestCleanup() { }

        protected async Task<List<IDataItem>> ReadData(IRavenDbSourceAdapterConfiguration configuration)
        {
            using (var adapter = await new RavenDbSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                return await ReadDataAsync(adapter);
            }
        }
    }
}
