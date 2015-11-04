using Microsoft.DataTransfer.Core.Statistics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DataTransfer.Core.UnitTests.Service
{
    [TestClass]
    public class TransferStatisticsTests
    {
        [TestMethod]
        public void AllMetrics_SampleData_Recorded()
        {
            const int NumberOfItems = 42;
            var errors = new KeyValuePair<string, Exception>[]
            {
                new KeyValuePair<string, Exception>("1", new Exception()),
                new KeyValuePair<string, Exception>("2", new ArgumentException()),
                new KeyValuePair<string, Exception>("3", new KeyNotFoundException())
            };

            var statistics = new InMemoryTransferStatistics(ErrorDetailsProviderMock.Instance);

            statistics.Start();

            for (var index = 0; index < NumberOfItems; ++index)
                statistics.AddTransferred();

            foreach (var error in errors)
                statistics.AddError(error.Key, error.Value);

            statistics.Stop();

            var result = statistics.GetSnapshot();

            Assert.IsNotNull(result, TestResources.NullStatisticsSnapshot);
            Assert.AreNotEqual(TimeSpan.Zero, result.ElapsedTime, TestResources.StatisticsElapsedTimeIsEmpty);
            Assert.AreEqual(NumberOfItems, result.Transferred, TestResources.StatisticsInvalidTransferredCount);
            Assert.AreEqual(errors.Length, result.Failed, TestResources.StatisticsInvalidFailedCount);
            CollectionAssert.AreEquivalent(errors.ToDictionary(e => e.Key, e => ErrorDetailsProviderMock.Instance.Get(e.Value)), result.GetErrors().ToArray(),
                TestResources.StatisticsInvalidErrors);
        } 
    }
}
