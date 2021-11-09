using Microsoft.DataTransfer.Basics.Collections;
using Microsoft.DataTransfer.DocumentDb.Transformation.Remap;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DataTransfer.DocumentDb.UnitTests.Sink
{
    [TestClass]
    public class RemappedFieldsDataItemTests
    {
        [TestMethod]
        public void GetFieldNames_FlatDocument_SingleFieldRenamed()
        {
            var dataItem = new RemappedFieldsDataItem(new DictionaryDataItem(new Dictionary<string, object>
                {
                    { "source", "Hello World!" },
                    { "persisted", 42 }
                }),
                new Map<string, string> { { "source", "target" } });

            var fieldNames = dataItem.GetFieldNames();
            Assert.IsNotNull(fieldNames, TestResources.NullRemappedFieldNames);
            CollectionAssert.AreEquivalent(new [] { "target", "persisted" }, fieldNames.ToArray(), TestResources.InvalidRemappedFieldNames);

            Assert.AreEqual("Hello World!", dataItem.GetValue("target"), TestResources.InvalidRemappedFieldValue);
            Assert.AreEqual(42, dataItem.GetValue("persisted"), TestResources.InvalidRemappedFieldValue);
        }

        [TestMethod]
        public void GetFieldNames_FlatDocument_MultipleFieldsRenamed()
        {
            var dataItem = new RemappedFieldsDataItem(new DictionaryDataItem(new Dictionary<string, object>
                {
                    { "source1", "Hello World!" },
                    { "source2", 42 }
                }),
                new Map<string, string>
                {
                    { "source1", "target1" },
                    { "source2", "target2" }
                });

            var fieldNames = dataItem.GetFieldNames();
            Assert.IsNotNull(fieldNames, TestResources.NullRemappedFieldNames);
            CollectionAssert.AreEquivalent(new[] { "target1", "target2" }, fieldNames.ToArray(), TestResources.InvalidRemappedFieldNames);

            Assert.AreEqual("Hello World!", dataItem.GetValue("target1"), TestResources.InvalidRemappedFieldValue);
            Assert.AreEqual(42, dataItem.GetValue("target2"), TestResources.InvalidRemappedFieldValue);
        }

        [TestMethod]
        public void GetFieldNames_FlatDocument_IgnoreRenamingOfNonexistingField()
        {
            var dataItem = new RemappedFieldsDataItem(new DictionaryDataItem(new Dictionary<string, object>
                {
                    { "source", "Hello World!" },
                    { "persisted", 42 }
                }),
                new Map<string, string> { { "nonexisting", "target" } });

            var fieldNames = dataItem.GetFieldNames();
            Assert.IsNotNull(fieldNames, TestResources.NullRemappedFieldNames);
            CollectionAssert.AreEquivalent(new[] { "source", "persisted" }, fieldNames.ToArray(), TestResources.InvalidRemappedFieldNames);

            Assert.AreEqual("Hello World!", dataItem.GetValue("source"), TestResources.InvalidRemappedFieldValue);
            Assert.AreEqual(42, dataItem.GetValue("persisted"), TestResources.InvalidRemappedFieldValue);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetFieldNames_FlatDocument_FieldCannotBeRenamedToExisting()
        {
            var dataItem = new RemappedFieldsDataItem(new DictionaryDataItem(new Dictionary<string, object>
                {
                    { "source", "Hello World!" },
                    { "persisted", 42 }
                }),
                new Map<string, string> { { "source", "persisted" } });

            var fieldNames = dataItem.GetFieldNames();
            Assert.IsNotNull(fieldNames, TestResources.NullRemappedFieldNames);
            CollectionAssert.AreEquivalent(new[] { "persisted" }, fieldNames.ToArray(), TestResources.InvalidRemappedFieldNames);

            Assert.AreEqual("Hello World!", dataItem.GetValue("persisted"), TestResources.InvalidRemappedFieldValue);
        }

        [TestMethod]
        public void GetFieldNames_FlatDocument_IgnoreRenamingOfNonexistingFieldIntoExisting()
        {
            var dataItem = new RemappedFieldsDataItem(new DictionaryDataItem(new Dictionary<string, object>
                {
                    { "source", "Hello World!" },
                    { "persisted", 42 }
                }),
                new Map<string, string> { { "nonexisting", "persisted" } });

            var fieldNames = dataItem.GetFieldNames();
            Assert.IsNotNull(fieldNames, TestResources.NullRemappedFieldNames);
            CollectionAssert.AreEquivalent(new[] { "source", "persisted" }, fieldNames.ToArray(), TestResources.InvalidRemappedFieldNames);

            Assert.AreEqual("Hello World!", dataItem.GetValue("source"), TestResources.InvalidRemappedFieldValue);
            Assert.AreEqual(42, dataItem.GetValue("persisted"), TestResources.InvalidRemappedFieldValue);
        }
    }
}
