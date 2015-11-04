using System.Collections.Generic;
using System.Spatial;

namespace Microsoft.DataTransfer.TestsCommon.SampleData
{
    public interface IGeospatialSampleDataProvider
    {
        Geography AsPoint(GeographyPosition position);
        Geography AsLineString(IEnumerable<GeographyPosition> positions);
        Geography AsPolygon(IEnumerable<GeographyPosition> positions);
        Geography AsMultiPoint(IEnumerable<GeographyPosition> positions);
        Geography AsMultiLineString(IEnumerable<IEnumerable<GeographyPosition>> positions);
        Geography AsMultiPolygon(IEnumerable<IEnumerable<GeographyPosition>> positions);
    }
}
