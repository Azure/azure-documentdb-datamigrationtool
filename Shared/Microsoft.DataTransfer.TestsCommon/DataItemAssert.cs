using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Microsoft.DataTransfer.TestsCommon
{
    public static class DataItemAssert
    {
        public static void AreEqual(IReadOnlyDictionary<string, object> expected, IDataItem actual)
        {
            AreEqual(new DictionaryDataItem(expected), actual);
        }

        public static void AreEqual(IDataItem expected, IDataItem actual)
        {
            var expectedNames = expected.GetFieldNames();
            Assert.IsNotNull(expectedNames, Resources.NullExpectedFieldNames);

            var actualNames = actual.GetFieldNames();
            Assert.IsNotNull(actualNames, Resources.NullActualFieldNames);

            CollectionAssert.AreEquivalent(expectedNames.ToArray(), actualNames.ToArray(), Resources.InvalidFieldNames);

            foreach (var fieldName in expectedNames)
            {
                var expectedValue = expected.GetValue(fieldName);
                var actualValue = actual.GetValue(fieldName);

                CheckItem(expectedValue, actualValue);
            }
        }

        private static void CheckItem(object expectedValue, object actualValue)
        {
            if (expectedValue is IDataItem)
            {
                Assert.IsTrue(actualValue is IDataItem, Resources.InvalidDataItemFieldValue);
                AreEqual((IDataItem)expectedValue, (IDataItem)actualValue);
            }
            else if (expectedValue is IEnumerable && !(expectedValue is string))
            {
                var expectedArray = ((IEnumerable)expectedValue).OfType<object>().ToArray();

                Assert.IsTrue(actualValue is IEnumerable, Resources.InvalidFieldValue);
                var actualArray = ((IEnumerable)actualValue).OfType<object>().ToArray();

                Assert.AreEqual(expectedArray.Length, actualArray.Length, Resources.InvalidNumberOfItems);

                for (int i = 0; i < expectedArray.Length; i++)
                {
                    CheckItem(expectedArray[i], actualArray[i]);
                }
            }
            else
            {
                if (expectedValue != null && expectedValue.GetType().IsValueType)
                    // Try change type to expected for value types to workaround DocumentDB deserialization issue (Int32 are becoming Int64)
                    try { actualValue = Convert.ChangeType(actualValue, expectedValue.GetType(), CultureInfo.InvariantCulture); } catch { }
                Assert.AreEqual(expectedValue, actualValue, Resources.InvalidFieldValue);
            }
        }
    }
}
