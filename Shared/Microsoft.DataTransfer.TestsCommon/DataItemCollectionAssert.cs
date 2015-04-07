using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DataTransfer.TestsCommon
{
    public static class DataItemCollectionAssert
    {
        public static void AreEquivalent(IEnumerable<IReadOnlyDictionary<string, object>> expected, IEnumerable<IDataItem> actual, string message)
        {
            AreEquivalent(expected.Select(i => new DictionaryDataItem(i)), actual, message);
        }

        public static void AreEquivalent(IEnumerable<IDataItem> expected, IEnumerable<IDataItem> actual, string message)
        {
            CollectionAssert.AreEquivalent(
                expected.Select(i => new ComparableDataItem(i)).ToArray(),
                actual.Select(i => new ComparableDataItem(i)).ToArray(),
                message);
        }
    }
}
