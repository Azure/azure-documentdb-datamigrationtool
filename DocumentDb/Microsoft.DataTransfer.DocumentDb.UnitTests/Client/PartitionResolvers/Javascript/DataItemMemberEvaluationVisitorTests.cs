using Microsoft.DataTransfer.DocumentDb.Client.PartitionResolvers.Javascript;
using Microsoft.DataTransfer.DocumentDb.Client.PartitionResolvers.Javascript.Visitors;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.UnitTests.Client.PartitionResolvers.Javascript
{
    [TestClass]
    public class DataItemMemberEvaluationVisitorTests
    {
        [TestMethod]
        public void GetAccessor_DirectMember_CanEvaluate()
        {
            var dataItem = new DictionaryDataItem(new Dictionary<string, object>
            {
                { "prop1", "Hello world!" },
                { "prop2", 10 },
            });

            Assert.AreEqual("Hello world!", EvaluateProperty(dataItem, "prop1"), TestResources.InvalidValueFromEvaluatedExpression);
        }

        [TestMethod]
        public void GetAccessor_NestedMember_CanEvaluate()
        {
            var dataItem = new DictionaryDataItem(new Dictionary<string, object>
            {
                { "prop1", new Dictionary<string, object> 
                    {
                        { "nested", 42 }
                    }
                },
                { "prop2", "some value" },
            });

            Assert.AreEqual(42, EvaluateProperty(dataItem, "prop1.nested"), TestResources.InvalidValueFromEvaluatedExpression);
        }

        [TestMethod]
        public void GetAccessor_DictionaryStyleExpression_CanEvaluate()
        {
            var dataItem = new DictionaryDataItem(new Dictionary<string, object>
            {
                { "prop1", new Dictionary<string, object> 
                    {
                        { "nested", 100 }
                    }
                },
                { "prop2", "some value" },
            });

            Assert.AreEqual(100, EvaluateProperty(dataItem, "prop1.['nested']"), TestResources.InvalidValueFromEvaluatedExpression);
        }

        [TestMethod]
        public void GetAccessor_EscapedDictionaryStyleExpression_CanEvaluate()
        {
            var dataItem = new DictionaryDataItem(new Dictionary<string, object>
            {
                { "prop1", new Dictionary<string, object> 
                    {
                        { "nes'ted", "Hello!" }
                    }
                },
                { "prop2", "some value" },
            });

            Assert.AreEqual("Hello!", EvaluateProperty(dataItem, @"prop1.['nes\'ted']"), TestResources.InvalidValueFromEvaluatedExpression);
        }

        [TestMethod]
        public void GetAccessor_ArrayMemberExpression_CanEvaluate()
        {
            var dataItem = new DictionaryDataItem(new Dictionary<string, object>
            {
                { "prop1", new[] { "Item1", "Item2", "Item3" } },
                { "prop2", "some value" },
            });

            Assert.AreEqual("Item3", EvaluateProperty(dataItem, "prop1[2]"), TestResources.InvalidValueFromEvaluatedExpression);
        }

        [TestMethod]
        public void GetAccessor_ArrayMemberExpressionWithNestedDataItem_CanEvaluate()
        {
            var dataItem = new DictionaryDataItem(new Dictionary<string, object>
            {
                { "prop1", new object[]
                    {
                        "Item1",
                        "Item2",
                        new Dictionary<string, object>
                        {
                            { "nested", "test" },
                            { "nested2", "another" }
                        }
                    }
                },
                { "prop2", "some value" },
            });

            Assert.AreEqual("another", EvaluateProperty(dataItem, "prop1[2].nested2"), TestResources.InvalidValueFromEvaluatedExpression);
        }

        private static object EvaluateProperty(IDataItem dataItem, string expression)
        {
            var visitor = new DataItemMemberEvaluationVisitor();
            new JavascriptMemberExpression(expression).Accept(visitor);
            return visitor.GetAccessor()(dataItem);
        }
    }
}
