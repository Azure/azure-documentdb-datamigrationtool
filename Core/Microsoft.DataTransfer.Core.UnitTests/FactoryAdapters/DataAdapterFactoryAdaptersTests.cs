using Microsoft.DataTransfer.Core.FactoryAdapters;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Core.UnitTests.FactoryAdapters
{
    [TestClass]
    public class DataAdapterFactoryAdaptersTests
    {
        [TestMethod]
        public async Task CreateAsync_DataSourceAdapter_Created()
        {
            const string TestDisplayName = "TestSource";

            var adapterMock = Mocks.Of<IDataSourceAdapter>().First();

            var adapterFactoryMock = new Mock<IDataSourceAdapterFactory<ITestAdapterConfiguration>>();
            adapterFactoryMock
                .Setup(f => f.CreateAsync(It.IsAny<ITestAdapterConfiguration>(), It.IsAny<IDataTransferContext>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(adapterMock));

            var configuration =
                Mocks
                    .Of<ITestAdapterConfiguration>(c =>
                        c.Text == "Test" && c.Number == 42)
                    .First();

            var factoryAdapter = new DataSourceAdapterFactoryAdapter<ITestAdapterConfiguration>(adapterFactoryMock.Object, TestDisplayName);

            var adapter = await factoryAdapter.CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None);

            Assert.AreEqual(TestDisplayName, factoryAdapter.DisplayName, TestResources.InvalidDataAdapter);

            Assert.IsNotNull(factoryAdapter.ConfigurationType, TestResources.EmptyConfigurationType);
            Assert.AreEqual(typeof(ITestAdapterConfiguration), factoryAdapter.ConfigurationType, TestResources.InvalidConfigurationType);

            Assert.AreEqual(adapterMock, adapter, TestResources.InvalidDataAdapter);
        }

        [TestMethod]
        public async Task CreateAsync_DataSinkAdapter_Created()
        {
            const string TestDisplayName = "TestSink";
            var adapterMock = Mocks.Of<IDataSinkAdapter>().First();

            var adapterFactoryMock = new Mock<IDataSinkAdapterFactory<ITestAdapterConfiguration>>();
            adapterFactoryMock
                .Setup(f => f.CreateAsync(It.IsAny<ITestAdapterConfiguration>(), It.IsAny<IDataTransferContext>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(adapterMock));

            var configuration =
                Mocks
                    .Of<ITestAdapterConfiguration>(c =>
                        c.Text == "Test" && c.Number == 42)
                    .First();

            var factoryAdapter = new DataSinkAdapterFactoryAdapter<ITestAdapterConfiguration>(adapterFactoryMock.Object, TestDisplayName);

            var adapter = await factoryAdapter.CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None);

            Assert.AreEqual(TestDisplayName, factoryAdapter.DisplayName, TestResources.InvalidDataAdapter);

            Assert.IsNotNull(factoryAdapter.ConfigurationType, TestResources.EmptyConfigurationType);
            Assert.AreEqual(typeof(ITestAdapterConfiguration), factoryAdapter.ConfigurationType, TestResources.InvalidConfigurationType);

            Assert.AreEqual(adapterMock, adapter, TestResources.InvalidDataAdapter);
        }
    }
}
