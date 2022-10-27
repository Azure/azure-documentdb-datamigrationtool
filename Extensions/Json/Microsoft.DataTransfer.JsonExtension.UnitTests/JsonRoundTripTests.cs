using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;

namespace Microsoft.DataTransfer.JsonExtension.UnitTests
{
    [TestClass]
    public class JsonRoundTripTests
    {
        [TestMethod]
        public async Task WriteAsync_fromReadAsync_ProducesIdenticalFile()
        {
            var input = new JsonDataSourceExtension();
            var output = new JsonDataSinkExtension();

            const string fileIn = "Data/ArraysTypesNesting.json";
            const string fileOut = $"{nameof(WriteAsync_fromReadAsync_ProducesIdenticalFile)}_out.json";
            
            var sourceConfig = TestHelpers.CreateConfig(new Dictionary<string, string>
            {
                { "FilePath", fileIn }
            });
            var sinkConfig = TestHelpers.CreateConfig(new Dictionary<string, string>
            {
                { "FilePath", fileOut },
                { "Indented", "true" },
            });

            await output.WriteAsync(input.ReadAsync(sourceConfig, NullLogger.Instance), sinkConfig, input, NullLogger.Instance);

            bool areEqual = JToken.DeepEquals(JToken.Parse(await File.ReadAllTextAsync(fileIn)), JToken.Parse(await File.ReadAllTextAsync(fileOut)));
            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public async Task WriteAsync_fromFolderReadAsync_ProducesExpectedCombinedFile()
        {
            var input = new JsonDataSourceExtension();
            var output = new JsonDataSinkExtension();

            const string fileIn = "Data/SingleObjects";
            const string fileCompare = "Data/SimpleIdName.json";
            const string fileOut = $"{nameof(WriteAsync_fromFolderReadAsync_ProducesExpectedCombinedFile)}_out.json";

            var sourceConfig = TestHelpers.CreateConfig(new Dictionary<string, string>
            {
                { "FilePath", fileIn }
            });
            var sinkConfig = TestHelpers.CreateConfig(new Dictionary<string, string>
            {
                { "FilePath", fileOut },
                { "Indented", "true" },
            });

            await output.WriteAsync(input.ReadAsync(sourceConfig, NullLogger.Instance), sinkConfig, input, NullLogger.Instance);

            bool areEqual = JToken.DeepEquals(JToken.Parse(await File.ReadAllTextAsync(fileCompare)), JToken.Parse(await File.ReadAllTextAsync(fileOut)));
            Assert.IsTrue(areEqual);
        }
    }
}