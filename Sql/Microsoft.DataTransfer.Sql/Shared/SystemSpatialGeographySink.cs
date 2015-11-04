using Microsoft.SqlServer.Types;
using System.Spatial;

namespace Microsoft.DataTransfer.Sql.Shared
{
    sealed class SystemSpatialGeographySink : IGeographySink110
    {
        private GeographyPipeline pipeline;

        public SystemSpatialGeographySink(GeographyPipeline targetPipeline)
        {
            this.pipeline = targetPipeline;
        }

        public void AddCircularArc(double x1, double y1, double? z1, double? m1, double x2, double y2, double? z2, double? m2)
        {
            throw Errors.CircularArcGeometryNotSupported();
        }

        public void AddLine(double latitude, double longitude, double? z, double? m)
        {
            pipeline.LineTo(new GeographyPosition(latitude, longitude, z, m));
        }

        public void BeginFigure(double latitude, double longitude, double? z, double? m)
        {
            pipeline.BeginFigure(new GeographyPosition(latitude, longitude, z, m));
        }

        public void BeginGeography(OpenGisGeographyType type)
        {
            pipeline.BeginGeography(ToSystemSpatialType(type));
        }

        private SpatialType ToSystemSpatialType(OpenGisGeographyType type)
        {
            switch (type)
            {
                case OpenGisGeographyType.Point:
                    return SpatialType.Point;
                case OpenGisGeographyType.LineString:
                    return SpatialType.LineString;
                case OpenGisGeographyType.Polygon:
                    return SpatialType.Polygon;
                case OpenGisGeographyType.MultiPoint:
                    return SpatialType.MultiPoint;
                case OpenGisGeographyType.MultiLineString:
                    return SpatialType.MultiLineString;
                case OpenGisGeographyType.MultiPolygon:
                    return SpatialType.MultiPolygon;
                case OpenGisGeographyType.GeometryCollection:
                    return SpatialType.Collection;
                case OpenGisGeographyType.FullGlobe:
                    return SpatialType.FullGlobe;
                default:
                    return SpatialType.Unknown;
            }
        }

        public void EndFigure()
        {
            pipeline.EndFigure();
        }

        public void EndGeography()
        {
            pipeline.EndGeography();
        }

        public void SetSrid(int srid)
        {
            pipeline.SetCoordinateSystem(CoordinateSystem.Geography(srid));
        }
    }
}
