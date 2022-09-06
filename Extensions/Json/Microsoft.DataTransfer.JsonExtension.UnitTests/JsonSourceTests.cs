using Microsoft.DataTransfer.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Microsoft.DataTransfer.JsonExtension.UnitTests
{
    [TestClass]
    public class JsonSourceTests
    {
        [TestMethod]
        public async Task ReadAsync_WithFlatObjects_ReadsValues()
        {
            var extension = new JsonDataSourceExtension();
            var config = TestHelpers.CreateConfig(new Dictionary<string, string>
            {
                { "FilePath", "Data/SimpleIdName.json" }
            });

            await foreach (var dataItem in extension.ReadAsync(config))
            {
                CollectionAssert.AreEquivalent(new[] { "id", "name" }, dataItem.GetFieldNames().ToArray());
                Assert.IsNotNull(dataItem.GetValue("id"));
                Assert.IsNotNull(dataItem.GetValue("name"));
            }
        }

        [TestMethod]
        public async Task ReadAsync_WithNestedObjects_ReadsValues()
        {
            var extension = new JsonDataSourceExtension();
            var config = TestHelpers.CreateConfig(new Dictionary<string, string>
            {
                { "FilePath", "Data/NestedObjects.json" }
            });

            await foreach (var dataItem in extension.ReadAsync(config))
            {
                if (dataItem.GetValue("child") is IDataItem child)
                {
                    CollectionAssert.AreEquivalent(new[] { "type", "data" }, child.GetFieldNames().ToArray());
                    Assert.IsNotNull(child.GetValue("type"));
                    Assert.IsNotNull(child.GetValue("data"));
                }
                else
                {
                    Assert.Fail();
                }
            }
        }
    }
}