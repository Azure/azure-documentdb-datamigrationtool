using System;
using System.Collections.Generic;
using System.Spatial;

namespace Microsoft.DataTransfer.TestsCommon.SampleData
{
    sealed class GeospatialSampleDataProvider : IGeospatialSampleDataProvider
    {
        public Geography AsPoint(GeographyPosition position)
        {
            return CreateGeography(SpatialType.Point, position, WritePoint);
        }

        public Geography AsLineString(IEnumerable<GeographyPosition> positions)
        {
            return CreateGeography(SpatialType.LineString, positions, WriteLineString);
        }

        public Geography AsPolygon(IEnumerable<GeographyPosition> positions)
        {
            return CreateGeography(SpatialType.Polygon, positions, WriteLineString);
        }

        public Geography AsMultiPoint(IEnumerable<GeographyPosition> positions)
        {
            return CreateGeography(SpatialType.MultiPoint, positions, WriteMultiPoint);
        }

        public Geography AsMultiLineString(IEnumerable<IEnumerable<GeographyPosition>> positions)
        {
            return CreateGeography(SpatialType.MultiLineString, positions, WriteMultiLineString);
        }

        public Geography AsMultiPolygon(IEnumerable<IEnumerable<GeographyPosition>> positions)
        {
            return CreateGeography(SpatialType.MultiPolygon, positions, WriteMultiPolygon);
        }

        private Geography CreateGeography<T>(SpatialType type, T data, Action<GeographyPipeline, T> writer)
        {
            var builder = SpatialImplementation.CurrentImplementation.CreateBuilder();

            builder.GeographyPipeline.SetCoordinateSystem(CoordinateSystem.DefaultGeography);

            builder.GeographyPipeline.BeginGeography(type);
            writer(builder.GeographyPipeline, data);
            builder.GeographyPipeline.EndGeography();

            return builder.ConstructedGeography;
        }

        private void WritePoint(GeographyPipeline pipeline, GeographyPosition position)
        {
            pipeline.BeginFigure(position);
            pipeline.EndFigure();
        }

        private void WriteLineString(GeographyPipeline pipeline, IEnumerable<GeographyPosition> positions)
        {
            var first = true;

            foreach (var position in positions)
            {
                if (first)
                {
                    pipeline.BeginFigure(position);
                    first = false;
                }
                else
                {
                    pipeline.LineTo(position);
                }
            }

            pipeline.EndFigure();
        }

        private void WriteMultiPoint(GeographyPipeline pipeline, IEnumerable<GeographyPosition> positions)
        {
            foreach (var position in positions)
            {
                pipeline.BeginFigure(position);
                pipeline.EndFigure();
            }
        }

        private void WriteMultiLineString(GeographyPipeline pipeline, IEnumerable<IEnumerable<GeographyPosition>> lineStrings)
        {
            foreach (var lineString in lineStrings)
                WriteLineString(pipeline, lineString);
        }

        private void WriteMultiPolygon(GeographyPipeline pipeline, IEnumerable<IEnumerable<GeographyPosition>> lineStrings)
        {
            pipeline.BeginGeography(SpatialType.Polygon);

            foreach (var lineString in lineStrings)
                WriteLineString(pipeline, lineString);

            pipeline.EndGeography();
        }
    }
}
