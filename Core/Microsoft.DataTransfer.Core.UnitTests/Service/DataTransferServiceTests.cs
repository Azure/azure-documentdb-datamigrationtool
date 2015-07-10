using Microsoft.DataTransfer.Basics.Threading;
using Microsoft.DataTransfer.Core.FactoryAdapters;
using Microsoft.DataTransfer.Core.Service;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.ServiceModel;
using Microsoft.DataTransfer.ServiceModel.Statistics;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Core.UnitTests.Service
{
    [TestClass]
    public class DataTransferServiceTests
    {
        [TestMethod]
        public async Task TransferAsync_TestDataAdapters_DisposeCalled()
        {
            const string SourceAdapterName = "TestSource";
            const string SinkAdapterName = "TestSink";

            var sourceMock = new Mock<IDataSourceAdapter>();
            sourceMock
                .Setup(m => m.ReadNextAsync(It.IsAny<ReadOutputByRef>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IDataItem>(null));
            sourceMock
                .Setup(m => m.Dispose())
                .Verifiable();

            var sinkMock = new Mock<IDataSinkAdapter>();
            sinkMock
                .Setup(m => m.WriteAsync(It.IsAny<IDataItem>(), It.IsAny<CancellationToken>()))
                .Returns(TaskHelper.NoOp);
            sinkMock
                .Setup(m => m.Dispose())
                .Verifiable();

            var service = new DataTransferService(
                    new Dictionary<string, IDataSourceAdapterFactoryAdapter>
                    {
                        { SourceAdapterName, Mocks.Of<IDataSourceAdapterFactoryAdapter>()
                            .Where(m => m.CreateAsync(It.IsAny<object>(), It.IsAny<IDataTransferContext>(), It.IsAny<CancellationToken>()) ==
                                Task.FromResult(sourceMock.Object)).First() }
                    },
                    new Dictionary<string, IDataSinkAdapterFactoryAdapter>
                    {
                        { SinkAdapterName, Mocks.Of<IDataSinkAdapterFactoryAdapter>()
                            .Where(m => m.CreateAsync(It.IsAny<object>(), It.IsAny<IDataTransferContext>(), It.IsAny<CancellationToken>()) ==
                                Task.FromResult(sinkMock.Object)).First() }
                    },
                    Mocks.Of<IDataTransferAction>()
                        .Where(a => a.ExecuteAsync(
                            It.IsAny<IDataSourceAdapter>(), It.IsAny<IDataSinkAdapter>(),
                            It.IsAny<ITransferStatistics>(), It.IsAny<CancellationToken>()) == Task.FromResult<object>(null))
                        .First()
                );

            await service.TransferAsync(
                SourceAdapterName, null, SinkAdapterName, null,
                new DummyTransferStatisticsMock(),
                CancellationToken.None);

            sourceMock.Verify();
            sinkMock.Verify();
        }

        [TestMethod]
        public async Task TransferAsync_TestDataAdapter_DataTransferContextPassed()
        {
            const string SourceAdapterName = "TestSource";
            const string SinkAdapterName = "TestSink";

            var sourceMock = new Mock<IDataSourceAdapter>();
            sourceMock
                .Setup(m => m.ReadNextAsync(It.IsAny<ReadOutputByRef>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IDataItem>(null));

            var sinkMock = new Mock<IDataSinkAdapter>();
            sinkMock
                .Setup(m => m.WriteAsync(It.IsAny<IDataItem>(), It.IsAny<CancellationToken>()))
                .Returns(TaskHelper.NoOp);

            var sourceFactoryMock = new Mock<IDataSourceAdapterFactoryAdapter>();
            sourceFactoryMock
                .Setup(m => m.CreateAsync(It.IsAny<object>(), It.IsAny<IDataTransferContext>(), It.IsAny<CancellationToken>()))
                .Callback<object, IDataTransferContext, CancellationToken>((a, c, ct) => 
                    Assert.AreEqual(SourceAdapterName, c.SourceName, TestResources.InvalidDataSourceNameInTransferContext))
                .Returns(Task.FromResult(sourceMock.Object));

            var sinkFactoryMock = new Mock<IDataSinkAdapterFactoryAdapter>();
            sinkFactoryMock
                .Setup(m => m.CreateAsync(It.IsAny<object>(), It.IsAny<IDataTransferContext>(), It.IsAny<CancellationToken>()))
                .Callback<object, IDataTransferContext, CancellationToken>((a, c, ct) => 
                    Assert.AreEqual(SinkAdapterName, c.SinkName, TestResources.InvalidDataSinkNameInTransferContext))
                .Returns(Task.FromResult(sinkMock.Object));

            var service = new DataTransferService(
                    new Dictionary<string, IDataSourceAdapterFactoryAdapter>
                    {
                        { SourceAdapterName, sourceFactoryMock.Object }
                    },
                    new Dictionary<string, IDataSinkAdapterFactoryAdapter>
                    {
                        { SinkAdapterName, sinkFactoryMock.Object }
                    },
                    Mocks.Of<IDataTransferAction>()
                        .Where(a => a.ExecuteAsync(
                            It.IsAny<IDataSourceAdapter>(), It.IsAny<IDataSinkAdapter>(),
                            It.IsAny<ITransferStatistics>(), It.IsAny<CancellationToken>()) == Task.FromResult<object>(null))
                        .First()
                );

            await service.TransferAsync(
                SourceAdapterName, null, SinkAdapterName, null,
                new DummyTransferStatisticsMock(),
                CancellationToken.None);
        }
    }
}
