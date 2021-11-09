using Microsoft.DataTransfer.DocumentDb.Transformation.Filter;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DataTransfer.DocumentDb.UnitTests.Transformation
{
    [TestClass]
    public class FilteredFieldsDataItemTests
    {
        [TestMethod]
        public void GetFieldNames_NestedDocument_TopLevelFieldsFiltered()
        {
            var dataItem = new FilteredFieldsDataItem(GetSampleDataItem(), new HashSet<string> { "toRemove" });

            var fieldNames = dataItem.GetFieldNames();
            Assert.IsNotNull(fieldNames, TestResources.NullFilteredFieldNames);
            CollectionAssert.AreEquivalent(new[] { "persisted", "nested" }, fieldNames.ToArray(),
                TestResources.InvalidFilteredFieldNames);

            Assert.AreEqual(42, dataItem.GetValue("persisted"), TestResources.InvalidFilteredFieldValue);

            var nestedDataItem = dataItem.GetValue("nested") as IDataItem;
            Assert.IsNotNull(nestedDataItem, TestResources.NullNestedDataItem);

            fieldNames = nestedDataItem.GetFieldNames();
            CollectionAssert.AreEquivalent(new[] { "toRemove", "persisted" }, fieldNames.ToArray(),
                TestResources.InvalidFilteredFieldNames);
            Assert.AreEqual("Hello Nested World!", nestedDataItem.GetValue("toRemove"), TestResources.InvalidFilteredFieldValue);
            Assert.AreEqual(20, nestedDataItem.GetValue("persisted"), TestResources.InvalidFilteredFieldValue);
        }

        private IDataItem GetSampleDataItem()
        {
            return new DictionaryDataItem(new Dictionary<string, object>
            {
                { "toRemove", "Hello World!" },
                { "persisted", 42 },
                { "nested", new Dictionary<string, object> 
                    {
                        { "toRemove", "Hello Nested World!" },
                        { "persisted", 20 }
                    }
                }
            });
        }
    }
}
