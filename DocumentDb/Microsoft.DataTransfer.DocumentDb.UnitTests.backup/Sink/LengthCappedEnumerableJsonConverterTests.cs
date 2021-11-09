using Microsoft.DataTransfer.DocumentDb.Sink.Bulk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Microsoft.DataTransfer.DocumentDb.UnitTests.Sink
{
    [TestClass]
    public class LengthCappedEnumerableJsonConverterTests
    {
        [TestMethod]
        public void SerializeLongCollection_CapedAfterSpecifiedLimit()
        {
            var data = new[]
            {
                new { Field1 = "Hello", Field2 = "World!" },
                new { Field1 = "Next", Field2 = "Item" },
                new { Field1 = "Should", Field2 = "cap" }
            };

            var cappable = new LengthCappedEnumerableSurrogate(data, 10, 80);
            var serialized = JsonConvert.SerializeObject(cappable, new LengthCappedEnumerableJsonConverter());

            Assert.AreEqual("[{\"Field1\":\"Hello\",\"Field2\":\"World!\"},{\"Field1\":\"Next\",\"Field2\":\"Item\"}]", serialized,
                TestResources.InvalidCappedEnumerableSerialization);
        }
    }
}
