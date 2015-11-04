using Microsoft.DataTransfer.Core.Service;
using Microsoft.DataTransfer.Core.Statistics;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Core.UnitTests.Service
{
    [TestClass]
    public class DataTransferActionTests : DataTransferTestBase
    {
        [TestMethod]
        public async Task ExecuteAsync_SampleData_Transferred()
        {
            const int NumberOfItems = 10;

            var action = new DataTransferAction();

            var sourceData = SampleData.GetSimpleDataItems(NumberOfItems);
            var sinkMock = new DataSinkAdapterMock();

            using (var source = new DataSourceAdapterMock(sourceData))
            using (var sink = sinkMock)
                await action.ExecuteAsync(source, sink, new DummyTransferStatisticsMock(), CancellationToken.None);

            var receivedData = sinkMock.ReceivedData;

            Assert.IsNotNull(receivedData, TestResources.NullDataInSink);

            CollectionAssert.AreEquivalent(
                sourceData,
                receivedData.ToArray(),
                TestResources.ReceivedDataDoesNotMatch);
        }

        [TestMethod]
        public async Task ExecuteAsync_SampleData_StatisticsPopulated()
        {
            const int NumberOfItems = 10;
            const int ThrowAfter = 5;

            var action = new DataTransferAction();

            var sourceData = SampleData.GetSimpleDataItems(NumberOfItems);

            var transferredCount = 0;
            var sinkMock = new DataSinkAdapterMock(i =>
                {
                    if (++transferredCount > ThrowAfter)
                        throw new Exception();
                });

            var statistics = new InMemoryTransferStatistics(ErrorDetailsProviderMock.Instance);
            statistics.Start();
            using (var source = new DataSourceAdapterMock(sourceData))
            using (var sink = sinkMock)
                await action.ExecuteAsync(source, sink, statistics, CancellationToken.None);
            statistics.Stop();

            var receivedData = sinkMock.ReceivedData;

            Assert.IsNotNull(receivedData, TestResources.NullDataInSink);
            Assert.AreEqual(ThrowAfter, receivedData.Count(), TestResources.InvalidNumberOfDataItems);

            var resultStatistics = statistics.GetSnapshot();
            Assert.AreEqual(ThrowAfter, resultStatistics.Transferred, TestResources.StatisticsInvalidTransferredCount);
            Assert.AreEqual(NumberOfItems - ThrowAfter, resultStatistics.Failed, TestResources.StatisticsInvalidFailedCount);
            Assert.AreNotEqual(TimeSpan.Zero, resultStatistics.ElapsedTime, TestResources.StatisticsElapsedTimeIsEmpty);

            var resultExceptions = resultStatistics.GetErrors();
            Assert.IsNotNull(resultExceptions, TestResources.NullTransferExceptions);
            Assert.AreEqual(NumberOfItems - ThrowAfter, resultExceptions.Count, TestResources.InvalidNumberOfTransferExceptions);
        }

        [TestMethod]
        public async Task ExecuteAsync_SampleData_NonFatalReadExceptionsSkipped()
        {
            const int NumberOfItems = 10;
            const int ThrowAfter = 5;

            var action = new DataTransferAction();

            var sourceData = SampleData.GetSimpleDataItems(NumberOfItems);

            var transferredCount = 0;
            var sourceMock = new DataSourceAdapterMock(sourceData, i =>
                {
                    if (ThrowAfter <= ++transferredCount && transferredCount < NumberOfItems)
                        throw new NonFatalReadException();
                });
            var sinkMock = new DataSinkAdapterMock();

            var statistics = new InMemoryTransferStatistics(ErrorDetailsProviderMock.Instance);
            statistics.Start();
            using (var source = sourceMock)
            using (var sink = sinkMock)
                await action.ExecuteAsync(source, sink, statistics, CancellationToken.None);
            statistics.Stop();

            var receivedData = sinkMock.ReceivedData;

            Assert.IsNotNull(receivedData, TestResources.NullDataInSink);
            Assert.AreEqual(ThrowAfter, receivedData.Count(), TestResources.InvalidNumberOfDataItems);

            var resultStatistics = statistics.GetSnapshot();
            Assert.AreEqual(ThrowAfter, resultStatistics.Transferred, TestResources.StatisticsInvalidTransferredCount);
            Assert.AreEqual(NumberOfItems - ThrowAfter, resultStatistics.Failed, TestResources.StatisticsInvalidFailedCount);
            Assert.AreNotEqual(TimeSpan.Zero, resultStatistics.ElapsedTime, TestResources.StatisticsElapsedTimeIsEmpty);

            var resultExceptions = resultStatistics.GetErrors();
            Assert.IsNotNull(resultExceptions, TestResources.NullTransferExceptions);
            Assert.AreEqual(NumberOfItems - ThrowAfter, resultExceptions.Count, TestResources.InvalidNumberOfTransferExceptions);
        }
    }
}
