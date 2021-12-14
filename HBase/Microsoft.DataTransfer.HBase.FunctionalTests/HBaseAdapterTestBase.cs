using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.HBase.Source;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.HBase.FunctionalTests
{
    [TestClass]
    public abstract class HBaseAdapterTestBase : DataTransferAdapterTestBase
    {
        protected string ConnectionString { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            if (String.IsNullOrWhiteSpace(Settings.HBaseConnectionString))
            {
                Assert.Inconclusive("You must provide a connection string value for the HBaseConnectionString property in the Microsoft.DataTransfer.HBase.FunctionalTests/.runsettings file.");
            }
            ConnectionString = Settings.HBaseConnectionString;

            TestInitialize();
        }

        protected virtual void TestInitialize() { }

        [TestCleanup]
        public void Cleanup()
        {
            TestCleanup();
        }

        protected virtual void TestCleanup() { }

        protected async Task<List<IDataItem>> ReadData(IHBaseSourceAdapterConfiguration configuration)
        {
            using (var adapter = await new HBaseSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                return await ReadDataAsync(adapter);
            }
        }
    }
}