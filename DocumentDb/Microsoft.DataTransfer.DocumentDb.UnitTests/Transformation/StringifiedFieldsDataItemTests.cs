using Microsoft.DataTransfer.DocumentDb.Transformation.Stringify;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.UnitTests.Transformation
{
    [TestClass]
    public class StringifiedFieldsDataItemTests
    {
        [TestMethod]
        public void GetValue_SpecificIntegerField_Stringified()
        {
            var transformed = new StringifiedFieldsDataItem(
                new DictionaryDataItem(new Dictionary<string, object>
                {
                    { "FirstNumber", 10 },
                    { "SecondNumber", 42 },
                    { "Nested", new Dictionary<string, object>
                        {
                            { "SecondNumber", 50 }
                        }
                    }
                }),
                new[] { "SecondNumber" });

            CollectionAssert.AreEquivalent(new[] { "FirstNumber", "SecondNumber", "Nested" }, transformed.GetFieldNames().ToArray(),
                TestResources.InvalidFieldNames);

            Assert.AreEqual(10, transformed.GetValue("FirstNumber"), TestResources.InvalidFieldValue);
            Assert.AreEqual("42", transformed.GetValue("SecondNumber"), TestResources.InvalidFieldValue);

            var nested = transformed.GetValue("Nested") as IDataItem;

            Assert.IsNotNull(nested, TestResources.NullNestedDataItem);

            CollectionAssert.AreEquivalent(new[] { "SecondNumber" }, nested.GetFieldNames().ToArray(),
                TestResources.InvalidFieldNames);

            Assert.AreEqual(50, nested.GetValue("SecondNumber"), TestResources.InvalidFieldValue);
        }

        [TestMethod]
        public void GetValue_NothingToStringify_ReturnsUnmodifiedDataItem()
        {
            var transformed = new StringifiedFieldsDataItem(
                new DictionaryDataItem(new Dictionary<string, object>
                {
                    { "FirstNumber", 100 },
                    { "SecondNumber", 42 },
                    { "Nested", new Dictionary<string, object>
                        {
                            { "SecondNumber", 50 }
                        }
                    }
                }),
                new[] { "NonExisting" });

            CollectionAssert.AreEquivalent(new[] { "FirstNumber", "SecondNumber", "Nested" }, transformed.GetFieldNames().ToArray(),
                TestResources.InvalidFieldNames);

            Assert.AreEqual(100, transformed.GetValue("FirstNumber"), TestResources.InvalidFieldValue);
            Assert.AreEqual(42, transformed.GetValue("SecondNumber"), TestResources.InvalidFieldValue);

            var nested = transformed.GetValue("Nested") as IDataItem;

            Assert.IsNotNull(nested, TestResources.NullNestedDataItem);

            CollectionAssert.AreEquivalent(new[] { "SecondNumber" }, nested.GetFieldNames().ToArray(),
                TestResources.InvalidFieldNames);

            Assert.AreEqual(50, nested.GetValue("SecondNumber"), TestResources.InvalidFieldValue);
        }
    }
}
