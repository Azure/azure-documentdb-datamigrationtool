using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.Composition;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.DataTransfer.JsonExtension.Settings;

namespace Microsoft.DataTransfer.JsonExtension
{
    [Export(typeof(IDataSinkExtension))]
    public class JsonDataSinkExtension : IDataSinkExtension
    {
        public string DisplayName => "JSON";

        public async Task WriteAsync(IAsyncEnumerable<IDataItem> dataItems, IConfiguration config, CancellationToken cancellationToken = default)
        {
            var settings = config.Get<JsonSinkSettings>();
            settings.Validate();

            if (settings.FilePath != null)
            {
                await using var stream = File.Create(settings.FilePath);
                await using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
                {
                    Indented = settings.Indented
                });
                writer.WriteStartArray();

                await foreach (var item in dataItems.WithCancellation(cancellationToken))
                {
                    WriteDataItem(settings, writer, item);
                }

                writer.WriteEndArray();
            }
        }

        private static void WriteDataItem(JsonSinkSettings settings, Utf8JsonWriter writer, IDataItem item, string? objectName = null)
        {
            if (objectName != null)
            {
                writer.WriteStartObject(objectName);
            }
            else
            {
                writer.WriteStartObject();
            }

            foreach (string fieldName in item.GetFieldNames())
            {
                var fieldValue = item.GetValue(fieldName);
                if (fieldValue == null)
                {
                    if (settings.IncludeNullFields)
                    {
                        writer.WriteNull(fieldName);
                    }

                    continue;
                }

                if (fieldValue is IDataItem child)
                {
                    WriteDataItem(settings, writer, child, fieldName);
                }
                else if (fieldValue is IEnumerable<IDataItem> children)
                {
                    writer.WriteStartArray(fieldName);
                    foreach (IDataItem arrayItem in children)
                    {
                        WriteDataItem(settings, writer, arrayItem);
                    }
                    writer.WriteEndArray();
                }
                else if (TryGetNumber(fieldValue, out var number))
                {
                    writer.WriteNumber(fieldName, number);
                }
                else if (fieldValue is bool boolean)
                {
                    writer.WriteBoolean(fieldName, boolean);
                }
                else
                {
                    writer.WriteString(fieldName, fieldValue.ToString());
                }
            }

            writer.WriteEndObject();
        }

        private static bool TryGetNumber(object x, out double number)
        {
            if (x is float f)
            {
                number = f;
                return true;
            }
            if (x is double d)
            {
                number = d;
                return true;
            }
            if (x is decimal m)
            {
                number = (double) m;
                return true;
            }
            if (x is int i)
            {
                number = i;
                return true;
            }
            if (x is short s)
            {
                number = s;
                return true;
            }
            if (x is long l)
            {
                number = l;
                return true;
            }

            number = default;
            return false;
        }
    }
}