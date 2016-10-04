using Microsoft.DataTransfer.Core.Service;
using Microsoft.DataTransfer.JsonFile.Sink;
using Microsoft.DataTransfer.JsonFile.Source;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.FunctionalTests
{
    [TestClass]
    public class JsonFileFunctionalTests
    {
        [TestMethod, Timeout(120000)]
        [DeploymentItem(@"TestData\Test.json", @"InputData")]
        public async Task TransferFromJsonToJson_AllDataTransferred()
        {
            var transfer = new DataTransferAction();

            var sourceConfiguration =
                Mocks
                    .Of<IJsonFileSourceAdapterConfiguration>(c =>
                        c.Files == new[] { @"InputData\Test.json" })
                    .First();

            var sinkConfiguration =
                Mocks
                    .Of<IJsonFileSinkAdapterConfiguration>(c => 
                        c.File == @"OutputData\Test1.json" &&
                        c.Prettify == true)
                    .First();

            using (var source = await new JsonFileSourceAdapterFactory()
                .CreateAsync(sourceConfiguration, DataTransferContextMock.Instance, CancellationToken.None))
            using (var sink = await new JsonFileSinkAdapterFactory()
                .CreateAsync(sinkConfiguration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                await transfer.ExecuteAsync(source, sink, new DummyTransferStatisticsMock(), CancellationToken.None);
            }

            var resultFile = new FileInfo(@"OutputData\Test1.json");
            Assert.IsTrue(resultFile.Exists, TestResources.OutputFileMissing);
            Assert.IsTrue(resultFile.Length > 0, TestResources.OutputFileEmpty);
        }

        [TestMethod, Timeout(120000)]
        [DeploymentItem(@"TestData\Test.gz", @"InputData")]
        public async Task TransferFromCompressedJsonToJson_AllDataTransferred()
        {
            var transfer = new DataTransferAction();

            var sourceConfiguration =
                Mocks
                    .Of<IJsonFileSourceAdapterConfiguration>(c =>
                        c.Files == new[] { @"InputData\Test.gz" } &&
                        c.Decompress == true)
                    .First();

            var sinkConfiguration =
                Mocks
                    .Of<IJsonFileSinkAdapterConfiguration>(c =>
                        c.File == @"OutputData\Test2.json" &&
                        c.Prettify == true)
                    .First();

            using (var source = await new JsonFileSourceAdapterFactory()
                .CreateAsync(sourceConfiguration, DataTransferContextMock.Instance, CancellationToken.None))
            using (var sink = await new JsonFileSinkAdapterFactory()
                .CreateAsync(sinkConfiguration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                await transfer.ExecuteAsync(source, sink, new DummyTransferStatisticsMock(), CancellationToken.None);
            }

            var resultFile = new FileInfo(@"OutputData\Test2.json");
            Assert.IsTrue(resultFile.Exists, TestResources.OutputFileMissing);
            Assert.IsTrue(resultFile.Length > 0, TestResources.OutputFileEmpty);
        }

        [TestMethod, Timeout(120000)]
        [DeploymentItem(@"TestData\Test.json", @"InputData")]
        public async Task TransferFromJsonToCompressedJson_AllDataTransferred()
        {
            var transfer = new DataTransferAction();

            var sourceConfiguration =
                Mocks
                    .Of<IJsonFileSourceAdapterConfiguration>(c =>
                        c.Files == new[] { @"InputData\Test.json" })
                    .First();

            var sinkConfiguration =
                Mocks
                    .Of<IJsonFileSinkAdapterConfiguration>(c =>
                        c.File == @"OutputData\Test3.gz" &&
                        c.Prettify == true &&
                        c.Compress == true)
                    .First();

            using (var source = await new JsonFileSourceAdapterFactory()
                .CreateAsync(sourceConfiguration, DataTransferContextMock.Instance, CancellationToken.None))
            using (var sink = await new JsonFileSinkAdapterFactory()
                .CreateAsync(sinkConfiguration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                await transfer.ExecuteAsync(source, sink, new DummyTransferStatisticsMock(), CancellationToken.None);
            }

            var resultFile = new FileInfo(@"OutputData\Test3.gz");
            Assert.IsTrue(resultFile.Exists, TestResources.OutputFileMissing);
            Assert.IsTrue(resultFile.Length > 0, TestResources.OutputFileEmpty);
        }
    }
}
