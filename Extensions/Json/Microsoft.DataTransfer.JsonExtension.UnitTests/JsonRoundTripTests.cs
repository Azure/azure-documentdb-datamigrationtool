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

            await output.WriteAsync(input.ReadAsync(sourceConfig), sinkConfig);

            bool areEqual = JToken.DeepEquals(JToken.Parse(await File.ReadAllTextAsync(fileIn)), JToken.Parse(await File.ReadAllTextAsync(fileOut)));
            Assert.IsTrue(areEqual);
        }
    }
}