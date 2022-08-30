using Microsoft.DataTransfer.AzureTableAPIExtension.Data;

namespace Microsoft.DataTransfer.AzureTableAPIExtension.UnitTests.Data
{
    [TestClass]
    public class AzureTableAPIDataItemExtensionsTests
    {
        [TestMethod]
        public void AzureTableAPIDataItem_GetString_01()
        {
            var dataitem = new MockDataItem(new Dictionary<string, object>()
            {
                { "Name", "Chris" }
            });

            var entity = dataitem.ToTableEntity(null, null);
            Assert.IsTrue(entity.ContainsKey("Name"));
            Assert.AreEqual("Chris", entity.GetString("Name"));
        }

        [TestMethod]
        public void AzureTableAPIDataItem_GetInt32_01()
        {
            var dataitem = new MockDataItem(new Dictionary<string, object>()
            {
                { "Number", 123 }
            });

            var entity = dataitem.ToTableEntity(null, null);
            Assert.IsTrue(entity.ContainsKey("Number"));
            Assert.AreEqual(123, entity.GetInt32("Number"));
        }

        [TestMethod]
        public void AzureTableAPIDataItem_PartitionKey_Int_01()
        {
            var dataitem = new MockDataItem(new Dictionary<string, object>()
            {
                { "PartitionKey", 123 }
            });

            var entity = dataitem.ToTableEntity(null, null);
            Assert.AreEqual(1, entity.Keys.Count());
            Assert.AreEqual("123", entity.PartitionKey);
        }

        [TestMethod]
        public void AzureTableAPIDataItem_PartitionKey_Int_02()
        {
            var dataitem = new MockDataItem(new Dictionary<string, object>()
            {
                { "MyID", 123 }
            });

            var entity = dataitem.ToTableEntity("MyID", null);
            Assert.AreEqual(1, entity.Keys.Count());
            Assert.AreEqual("123", entity.PartitionKey);
        }

        [TestMethod]
        public void AzureTableAPIDataItem_PartitionKey_String_01()
        {
            var dataitem = new MockDataItem(new Dictionary<string, object>()
            {
                { "PartitionKey", "WI" }
            });

            var entity = dataitem.ToTableEntity(null, null);
            Assert.AreEqual(1, entity.Keys.Count());
            Assert.AreEqual("WI", entity.PartitionKey);
        }

        [TestMethod]
        public void AzureTableAPIDataItem_PartitionKey_String_02()
        {
            var dataitem = new MockDataItem(new Dictionary<string, object>()
            {
                { "MyVal", "WI" }
            });

            var entity = dataitem.ToTableEntity("MyVal", null);
            Assert.AreEqual(1, entity.Keys.Count());
            Assert.AreEqual("WI", entity.PartitionKey);
        }

        [TestMethod]
        public void AzureTableAPIDataItem_RowKey_Int_01()
        {
            var dataitem = new MockDataItem(new Dictionary<string, object>()
            {
                { "RowKey", 123 }
            });

            var entity = dataitem.ToTableEntity(null, null);
            Assert.AreEqual(1, entity.Keys.Count());
            Assert.AreEqual("123", entity.RowKey);
        }

        [TestMethod]
        public void AzureTableAPIDataItem_RowKey_Int_02()
        {
            var dataitem = new MockDataItem(new Dictionary<string, object>()
            {
                { "MyKey", 123 }
            });

            var entity = dataitem.ToTableEntity(null, "MyKey");
            Assert.AreEqual(1, entity.Keys.Count());
            Assert.AreEqual("123", entity.RowKey);
        }

        [TestMethod]
        public void AzureTableAPIDataItem_RowKey_String_01()
        {
            var dataitem = new MockDataItem(new Dictionary<string, object>()
            {
                { "RowKey", "WI" }
            });

            var entity = dataitem.ToTableEntity(null, null);
            Assert.AreEqual(1, entity.Keys.Count());
            Assert.AreEqual("WI", entity.RowKey);
        }

        [TestMethod]
        public void AzureTableAPIDataItem_RowKey_String_02()
        {
            var dataitem = new MockDataItem(new Dictionary<string, object>()
            {
                { "MyVal", "WI" }
            });

            var entity = dataitem.ToTableEntity(null, "MyVal");
            Assert.AreEqual(1, entity.Keys.Count());
            Assert.AreEqual("WI", entity.RowKey);
        }

        [TestMethod]
        public void AzureTableAPIDataItem_PartitionKey_RowKey_01()
        {
            var dataitem = new MockDataItem(new Dictionary<string, object>()
            {
                { "PartitionKey", "WI" },
                { "RowKey", 123 }
            });

            var entity = dataitem.ToTableEntity(String.Empty, String.Empty);
            Assert.AreEqual(2, entity.Keys.Count());
            Assert.AreEqual("WI", entity.PartitionKey);
            Assert.AreEqual("123", entity.RowKey);
        }

        [TestMethod]
        public void AzureTableAPIDataItem_PartitionKey_RowKey_02()
        {
            var dataitem = new MockDataItem(new Dictionary<string, object>()
            {
                { "MyVal", "Tailspin" },
                { "ID", 456 }
            });

            var entity = dataitem.ToTableEntity("MyVal", "ID");
            Assert.AreEqual(2, entity.Keys.Count());
            Assert.AreEqual("Tailspin", entity.PartitionKey);
            Assert.AreEqual("456", entity.RowKey);
        }
    }
}
