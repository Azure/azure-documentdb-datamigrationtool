using System.Collections.Generic;
using Microsoft.DataTransfer.Extensibility;

namespace Microsoft.DataTransfer.TestsCommon.SampleData
{
    public interface ISampleDataProvider
    {
        IDataItem[] GetSimpleDataItems(int count);
        Dictionary<string, object>[] GetSimpleDocuments(int count);
    }
}
