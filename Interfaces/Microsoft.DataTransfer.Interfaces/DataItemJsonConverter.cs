using System.Text;
using System.Text.Json;

namespace Microsoft.DataTransfer.Interfaces;

public static class DataItemJsonConverter
{
    public static string AsJsonString(this IDataItem dataItem, bool indented, bool includeNullFields)
    {
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = indented }))
        {
            WriteDataItem(writer, dataItem, includeNullFields);
        }

        var bytes = stream.ToArray();
        return Encoding.UTF8.GetString(bytes);
    }

    public static void WriteDataItem(Utf8JsonWriter writer, IDataItem item, bool includeNullFields, string? objectName = null)
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
            WriteFieldValue(writer, fieldName, fieldValue, includeNullFields);
        }

        writer.WriteEndObject();
    }

    private static void WriteFieldValue(Utf8JsonWriter writer, string fieldName, object? fieldValue, bool includeNullFields)
    {
        if (fieldValue == null)
        {
            if (includeNullFields)
            {
                writer.WriteNull(fieldName);
            }
        }
        else
        {
            if (fieldValue is IDataItem child)
            {
                WriteDataItem(writer, child, includeNullFields, fieldName);
            }
            else if (fieldValue is IEnumerable<object> children)
            {
                writer.WriteStartArray(fieldName);
                foreach (object arrayItem in children)
                {
                    if (arrayItem is IDataItem arrayChild)
                    {
                        WriteDataItem(writer, arrayChild, includeNullFields);
                    }
                    else if (TryGetNumber(arrayItem, out var number))
                    {
                        writer.WriteNumberValue(number);
                    }
                    else if (arrayItem is bool boolean)
                    {
                        writer.WriteBooleanValue(boolean);
                    }
                    else
                    {
                        writer.WriteStringValue(arrayItem.ToString());
                    }
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
            number = (double)m;
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