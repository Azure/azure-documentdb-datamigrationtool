using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.JsonNet.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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
                        : value.GetHashCode());
            }

            return result;
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
