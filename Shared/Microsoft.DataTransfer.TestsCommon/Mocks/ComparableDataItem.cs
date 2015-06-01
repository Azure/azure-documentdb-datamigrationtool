using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.JsonNet.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.DataTransfer.TestsCommon.Mocks
{
    sealed class ComparableDataItem : IDataItem
    {
        private IDataItem dataItem;
        private int hash;

        public ComparableDataItem(IDataItem dataItem)
        {
            if (dataItem == null)
                throw new ArgumentNullException("dataItem");

            this.dataItem = dataItem;
            hash = GetDataItemHash(dataItem);
        }

        private static int GetDataItemHash(IDataItem dataItem)
        {
            var result = 0;

            foreach (var fieldName in dataItem.GetFieldNames())
            {
                var value = dataItem.GetValue(fieldName);
                if (value == null)
                    continue;

                result ^= fieldName.GetHashCode() ^ (
                    value is IDataItem
                        ? GetDataItemHash((IDataItem)value)
                        : GetValueHashCode(value));
            }

            return result;
        }

        private static int GetValueHashCode(object value)
        {
            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Single:
                    // Convert all numeric types to double since type can change after deserialization
                    return Convert.ChangeType(value, typeof(double), CultureInfo.InvariantCulture).GetHashCode();
                default:
                    return value.GetHashCode();
            }
        }

        public IEnumerable<string> GetFieldNames()
        {
            return dataItem.GetFieldNames();
        }

        public object GetValue(string fieldName)
        {
            return dataItem.GetValue(fieldName);
        }

        public override bool Equals(object obj)
        {
            var other = obj as IDataItem;
            if (other == null)
                return false;

            // Lets take a shortcut ;)
            try
            {
                DataItemAssert.AreEqual(dataItem, other);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return hash;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(dataItem, DataItemJsonConverter.Instance);
        }
    }
}
