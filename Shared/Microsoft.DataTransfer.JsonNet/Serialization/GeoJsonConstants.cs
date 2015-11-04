
namespace Microsoft.DataTransfer.JsonNet.Serialization
{
    static class GeoJsonConstants
    {
        public static string TypePropertyName = "type";
        public static string CoordinatesPropertyName = "coordinates";
        public static string GeometriesPropertyName = "geometries";

        public static string CoordinateSystemPropertyName = "crs";
        public static string PropertiesPropertyName = "properties";
        public static string NamePropertyName = "name";

        public static class GeoTypes
        {
            public static string Point = "Point";
            public static string MultiPoint = "MultiPoint";
            public static string LineString = "LineString";
            public static string MultiLineString = "MultiLineString";
            public static string Polygon = "Polygon";
            public static string MultiPolygon = "MultiPolygon";
            public static string GeometryCollection = "GeometryCollection";
        }

        public static class CoordinateSystemTypes
        {
            public static string Name = "name";
        }
    }
}
