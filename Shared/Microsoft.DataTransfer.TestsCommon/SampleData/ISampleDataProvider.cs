using Microsoft.DataTransfer.Extensibility;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.TestsCommon.SampleData
{
    public interface ISampleDataProvider
    {
        IGeospatialSampleDataProvider Geospatial { get; }

        IDataItem[] GetSimpleDataItems(int count);
        Dictionary<string, object>[] GetSimpleDocuments(int count);
    }
}
