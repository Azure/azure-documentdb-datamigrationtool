using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DataTransfer.TestsCommon.SampleData
{
    sealed class SampleDataProvider : ISampleDataProvider
    {
        public IDataItem[] GetSimpleDataItems(int count)
        {
            var items = GetSimpleDocuments(count);
            return items.Select(i => new DictionaryDataItem(i)).ToArray();
        }

        public Dictionary<string, object>[] GetSimpleDocuments(int count)
        {
            var time = DateTime.Today.ToUniversalTime();
            return Enumerable
                .Range(0, count)
                .Select(i => new Dictionary<string, object>
                    {
                        { "id", Guid.NewGuid().ToString() },
                        { "StringProperty", "Value " + i },
                        { "IntegerProperty", i },
                        { "DateTimeProperty", time.AddHours(-i) },
                        { "BoolProperty", i % 2 == 0 },
                        { "FloatProperty", i + 0.123 }
                    })
                .ToArray();
        }
    }
}
