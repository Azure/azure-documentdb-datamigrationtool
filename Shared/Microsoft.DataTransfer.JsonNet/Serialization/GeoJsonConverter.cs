using Microsoft.DataTransfer.Basics;
using Newtonsoft.Json;
using System;
using System.Spatial;

namespace Microsoft.DataTransfer.JsonNet.Serialization
{
    /// <summary>
    /// Converts <see cref="Geography" /> to GeoJSON.
    /// </summary>
    public sealed class GeoJsonConverter : JsonConverter
    {
        /// <summary>
        /// Singleton instance of the converter.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes",
            Justification = "Immutable singleton instance")]
        public static readonly GeoJsonConverter Instance = new GeoJsonConverter();

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>true if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(Geography).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // TODO: Try to parse the object as geography element based on presence of "coordinates" and "type" elements?
            Guard.NotNull("serializer", serializer);
            return serializer.Deserialize(reader, objectType);
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Guard.NotNull("writer", writer);
            Guard.NotNull("serializer", serializer);

            var geography = value as Geography;
            if (geography == null)
                serializer.Serialize(writer, value);

            WriteGeography(writer, geography);
        }

        private static void WriteGeography(JsonWriter writer, Geography geography)
        {
            writer.WriteStartObject();

            if (geography is GeographyPoint)
            {
                WriteGeographyElement<GeographyPoint>(writer,
                    GeoJsonConstants.GeoTypes.Point, (GeographyPoint)geography, WritePointCoordinates);
            }
            else if (geography is GeographyLineString)
            {
                WriteGeographyElement<GeographyLineString>(writer,
                    GeoJsonConstants.GeoTypes.LineString, (GeographyLineString)geography, WriteLineStringCoordinates);
            }
            else if (geography is GeographyPolygon)
            {
                WriteGeographyElement<GeographyPolygon>(writer,
                    GeoJsonConstants.GeoTypes.Polygon, (GeographyPolygon)geography, WritePolygonCoordinates);
            }
            else if (geography is GeographyMultiPoint)
            {
                WriteGeographyElement<GeographyMultiPoint>(writer,
                    GeoJsonConstants.GeoTypes.MultiPoint, (GeographyMultiPoint)geography, WriteMultiPointCoordinates);
            }
            else if (geography is GeographyMultiPoint)
            {
                WriteGeographyElement<GeographyMultiLineString>(writer,
                    GeoJsonConstants.GeoTypes.MultiLineString, (GeographyMultiLineString)geography, WriteMultiLineStringCoordinates);
            }
            else if (geography is GeographyMultiPolygon)
            {
                WriteGeographyElement<GeographyMultiPolygon>(writer,
                    GeoJsonConstants.GeoTypes.MultiPolygon, (GeographyMultiPolygon)geography, WriteMultiPolygonCoordinates);
            }
            else if (geography is GeographyCollection)
            {
                WriteRawGeographyElement<GeographyCollection>(writer,
                    GeoJsonConstants.GeoTypes.GeometryCollection, (GeographyCollection)geography,
                    GeoJsonConstants.GeometriesPropertyName, WriteGeographyCollection);
            }
            else
            {
                throw Errors.GeometryTypeNotSupported(geography.GetType());
            }

            writer.WriteEndObject();
        }

        private static void WriteGeographyElement<T>(JsonWriter writer, string type, T element, Action<JsonWriter, T> coordinatesWriter)
            where T : Geography
        {
            WriteRawGeographyElement<T>(writer, type, element, GeoJsonConstants.CoordinatesPropertyName, coordinatesWriter);
        }

        private static void WriteRawGeographyElement<T>(JsonWriter writer, string type, T element, string childrenPropertyName, Action<JsonWriter, T> childrenWriter)
            where T : Geography
        {
            writer.WritePropertyName(GeoJsonConstants.TypePropertyName);
            writer.WriteValue(type);

            if (!element.IsEmpty)
            {
                writer.WritePropertyName(childrenPropertyName);
                childrenWriter(writer, element);
            }

            WriteCoordinateSystem(writer, element.CoordinateSystem);
        }

        private static void WriteCoordinateSystem(JsonWriter writer, CoordinateSystem coordinateSystem)
        {
            if (coordinateSystem == CoordinateSystem.DefaultGeography)
                return;

            writer.WriteStartObject();

            writer.WritePropertyName(GeoJsonConstants.TypePropertyName);
            writer.WriteValue(GeoJsonConstants.CoordinateSystemTypes.Name);

            writer.WritePropertyName(GeoJsonConstants.PropertiesPropertyName);
            writer.WriteStartObject();
            writer.WritePropertyName(GeoJsonConstants.NamePropertyName);
            writer.WriteValue(coordinateSystem.Name);
            writer.WriteEndObject();

            writer.WriteEndObject();
        }

        #region Primitives

        private static void WritePointCoordinates(JsonWriter writer, GeographyPoint point)
        {
            writer.WriteStartArray();

            writer.WriteValue(point.Longitude);
            writer.WriteValue(point.Latitude);

            if (point.Z.HasValue)
                writer.WriteValue(point.Z.Value);

            if (point.M.HasValue)
                writer.WriteValue(point.M.Value);

            writer.WriteEndArray();
        }

        private static void WriteLineStringCoordinates(JsonWriter writer, GeographyLineString lineString)
        {
            writer.WriteStartArray();

            foreach (var point in lineString.Points)
                WritePointCoordinates(writer, point);

            writer.WriteEndArray();
        }

        private static void WritePolygonCoordinates(JsonWriter writer, GeographyPolygon polygon)
        {
            writer.WriteStartArray();

            foreach (var ring in polygon.Rings)
                WriteLineStringCoordinates(writer, ring);

            writer.WriteEndArray();
        }

        private static void WriteMultiPointCoordinates(JsonWriter writer, GeographyMultiPoint multiPoint)
        {
            writer.WriteStartArray();

            foreach (var point in multiPoint.Points)
                WritePointCoordinates(writer, point);

            writer.WriteEndArray();
        }

        private static void WriteMultiLineStringCoordinates(JsonWriter writer, GeographyMultiLineString multiLineString)
        {
            writer.WriteStartArray();

            foreach (var lineString in multiLineString.LineStrings)
                WriteLineStringCoordinates(writer, lineString);

            writer.WriteEndArray();
        }

        private static void WriteMultiPolygonCoordinates(JsonWriter writer, GeographyMultiPolygon multiPolygon)
        {
            writer.WriteStartArray();

            foreach (var polygon in multiPolygon.Polygons)
                WritePolygonCoordinates(writer, polygon);

            writer.WriteEndArray();
        }

        private static void WriteGeographyCollection(JsonWriter writer, GeographyCollection geographyCollection)
        {
            writer.WriteStartArray();

            foreach (var geography in geographyCollection.Geographies)
                WriteGeography(writer, geography);

            writer.WriteEndArray();
        }

        #endregion Primitives
    }
}
