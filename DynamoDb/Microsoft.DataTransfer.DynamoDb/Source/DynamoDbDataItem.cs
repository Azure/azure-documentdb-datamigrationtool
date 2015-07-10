using Amazon.DynamoDBv2.Model;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Microsoft.DataTransfer.DynamoDb.Source
{
    sealed class DynamoDbDataItem : IDataItem
    {
        private readonly IReadOnlyDictionary<string, AttributeValue> data;

        public DynamoDbDataItem(IReadOnlyDictionary<string, AttributeValue> data)
        {
            this.data = data;
        }

        public IEnumerable<string> GetFieldNames()
        {
            return data.Keys;
        }

        public object GetValue(string fieldName)
        {
            Guard.NotNull("fieldName", fieldName);

            AttributeValue value;
            if (!data.TryGetValue(fieldName, out value))
                throw CommonErrors.DataItemFieldNotFound(fieldName);

            return ConvertAttributeValue(value);
        }

        private object ConvertAttributeValue(AttributeValue value)
        {
            if (value.N != null)
            {
                return ConvertNumber(value.N);
            }

            if (value.NS != null && value.NS.Count > 0)
            {
                var result = new double[value.NS.Count];
                for (var index = 0; index < result.Length; ++index)
                    result[index] = ConvertNumber(value.NS[index]);
                return result;
            }

            if (value.S != null)
            {
                return value.S;
            }

            if (value.SS != null && value.SS.Count > 0)
            {
                var result = new string[value.SS.Count];
                for (var index = 0; index < result.Length; ++index)
                    result[index] = value.SS[index];
                return result;
            }

            if (value.IsBOOLSet)
            {
                return value.BOOL;
            }

            if (value.IsLSet && value.L != null)
            {
                var result = new object[value.L.Count];
                for (var index = 0; index < result.Length; ++index)
                    result[index] = ConvertAttributeValue(value.L[index]);
                return result;
            }

            if (value.IsMSet && value.M != null)
            {
                return new DynamoDbDataItem(value.M);
            }

            if (value.B != null)
            {
                return ConvertBinaryData(value.B);
            }

            if (value.BS != null && value.BS.Count > 0)
            {
                var result = new byte[value.BS.Count][];
                for (var index = 0; index < result.Length; ++index)
                    result[index] = ConvertBinaryData(value.BS[index]);
                return result;
            }

            return null;
        }

        private static byte[] ConvertBinaryData(MemoryStream stream)
        {
            return stream.ToArray();
        }

        private static double ConvertNumber(string number)
        {
            return double.Parse(number, CultureInfo.InvariantCulture);
        }
    }
}
