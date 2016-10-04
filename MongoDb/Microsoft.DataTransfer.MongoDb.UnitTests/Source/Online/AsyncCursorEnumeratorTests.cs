using Microsoft.DataTransfer.Extensibility.Basics.Collections;
using Microsoft.DataTransfer.MongoDb.Source.Online;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.MongoDb.UnitTests.Source.Online
{
    [TestClass]
    public class AsyncCursorEnumeratorTests
    {
        [TestMethod]
        public async Task AsyncCursorEnumerator_ThreeBatches_ReadsAllData()
        {
            await RunTest(
                new[]
                {
                    new[] { 1, 2, 3, 4 },
                    new[] { 5, 6, 7, 8 },
                    new[] { 9, 10, 11, 12 }
                });
        }

        [TestMethod]
        public async Task AsyncCursorEnumerator_ThreeBatchesWithOneEmpty_ReadsAllData()
        {
            await RunTest(
                new[]
                {
                    new[] { 1, 2, 3, 4 },
                    new int[0],
                    new[] { 9, 10, 11, 12 }
                });
        }

        [TestMethod]
        public async Task AsyncCursorEnumerator_ThreeBatchesWithTwoEmpty_ReadsAllData()
        {
            await RunTest(
                new[]
                {
                    new[] { 1, 2, 3, 4 },
                    new int[0],
                    new int[0]
                });
        }

        [TestMethod]
        public async Task AsyncCursorEnumerator_TwoEmptyBatches_ReadsEmptyData()
        {
            await RunTest(
                new[]
                {
                    new int[0],
                    new int[0]
                });
        }

        [TestMethod]
        public async Task AsyncCursorEnumerator_ThreeBatchesWithOneNull_ReadsAllData()
        {
            await RunTest(
                new[]
                {
                    new[] { 1, 2, 3, 4 },
                    null,
                    new[] { 10, 20, 30, 40 }
                });
        }

        [TestMethod]
        public async Task AsyncCursorEnumerator_ThreeBatchesWithTwoNull_ReadsAllData()
        {
            await RunTest(
                new[]
                {
                    new[] { 1, 2, 3, 4 },
                    null,
                    null
                });
        }

        [TestMethod]
        public async Task AsyncCursorEnumerator_TwoNullBatches_ReadsEmpty()
        {
            await RunTest(
                new int[][]
                {
                    null,
                    null
                });
        }

        [TestMethod]
        public async Task AsyncCursorEnumerator_NoBatches_ReadsEmptyData()
        {
            await RunTest(new int[0][]);
        }

        private async Task RunTest<T>(IEnumerable<IEnumerable<T>> testData)
        {
            var cursorMock = new AsyncCursorMock<T>(testData);

            using (IAsyncEnumerator<T> cursorEnumerator =
                new AsyncCursorEnumerator<T>(cursorMock))
            {
                CollectionAssert.AreEqual(
                    testData.SelectMany(b => b ?? Enumerable.Empty<T>()).ToArray(),
                    await ReadToEnd(cursorEnumerator),
                    TestResources.AsyncCursorEnumeratorInvalidData);
            }

            Assert.IsTrue(cursorMock.Disposed, TestResources.AsyncCursorNotDisposed);
        }

        private async Task<List<T>> ReadToEnd<T>(IAsyncEnumerator<T> enumerator)
        {
            var result = new List<T>();

            while (await enumerator.MoveNextAsync(CancellationToken.None))
            {
                result.Add(enumerator.Current);
            }

            return result;
        }
    }
}
