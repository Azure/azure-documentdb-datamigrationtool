using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DataTransfer.DocumentDb.FunctionalTests
{
    [TestClass]
    public abstract class DocumentDbSinkAdapterTestBase : DocumentDbAdapterTestBase
    {
        protected static void VerifyData(IEnumerable<IDataItem> expected, IEnumerable<IReadOnlyDictionary<string, object>> actual)
        {
            var persistedData = actual
                .Select(i => new DictionaryDataItem(i
                    // Exclude all internal properties
                    .Where(p => !p.Key.StartsWith("_"))
                    .ToDictionary(p => p.Key, p => p.Value)))
                .ToList();

            DataItemCollectionAssert.AreEquivalent(expected, persistedData, TestResources.InvalidDocumentsPersisted);
        }
    }
}
