using Azure.Data.Tables;
using Microsoft.DataTransfer.AzureTableAPIExtension.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.AzureTableAPIExtension.UnitTests.Data
{
    [TestClass]
    public class AzureTableAPIDataItemTests
    {
        [TestMethod]
        public void AzureTableAPIDataItem_GetFieldNames_01()
        {
            var entity = new TableEntity(new Dictionary<string, object>()
            {
                { "ID", 1 },
                { "Name", "Chris" },
                { "State", "FL" }
            });

            var dataitem = new AzureTableAPIDataItem(entity, null, null);
            var keys = dataitem.GetFieldNames();

            Assert.AreEqual(3, keys.Count());
            Assert.IsTrue(keys.Contains("ID"), "Field 'ID' not found");
            Assert.IsTrue(keys.Contains("Name"), "Field 'Name' not found");
            Assert.IsTrue(keys.Contains("State"), "Field 'State' not found");
        }

        [TestMethod]
        public void AzureTableAPIDataItem_GetFieldNames_02()
        {
            var entity = new TableEntity(new Dictionary<string, object>()
            {
                { "Name", "Chris" }
            });
            entity.RowKey = "1";
            entity.PartitionKey = "FL";

            var dataitem = new AzureTableAPIDataItem(entity, null, null);
            var keys = dataitem.GetFieldNames();

            Assert.AreEqual(3, keys.Count());
            Assert.IsTrue(keys.Contains("RowKey"), "Field 'RowKey' not found");
            Assert.IsTrue(keys.Contains("PartitionKey"), "Field 'PartitionKey' not found");
            Assert.IsTrue(keys.Contains("Name"), "Field 'Name' not found");
        }

        [TestMethod]
        public void AzureTableAPIDataItem_Values_String_01()
        {
            var entity = new TableEntity(new Dictionary<string, object>()
            {
                { "Name", "Chris" }
            });

            var dataitem = new AzureTableAPIDataItem(entity, null, null);
            var keys = dataitem.GetFieldNames();

            Assert.AreEqual("Chris", dataitem.GetValue("Name"));
        }

        [TestMethod]
        public void AzureTableAPIDataItem_Values_Int_01()
        {
            var entity = new TableEntity(new Dictionary<string, object>()
            {
                { "ID", 110 }
            });

            var dataitem = new AzureTableAPIDataItem(entity, null, null);
            var keys = dataitem.GetFieldNames();

            Assert.AreEqual(110, dataitem.GetValue("ID"));
        }

        [TestMethod]
        public void AzureTableAPIDataItem_Values_Bool_01()
        {
            var entity = new TableEntity(new Dictionary<string, object>()
            {
                { "SomeValue", true }
            });

            var dataitem = new AzureTableAPIDataItem(entity, null, null);
            var keys = dataitem.GetFieldNames();

            Assert.IsTrue((bool?)dataitem.GetValue("SomeValue"));
        }
    }
}
