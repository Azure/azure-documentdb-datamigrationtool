using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.HBase.Client.Entities;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.HBase.Source
{
    sealed class HBaseCellsDataItem : IDataItem
    {
        private readonly IReadOnlyDictionary<string, HBaseCell> data;

        public HBaseCellsDataItem(IReadOnlyDictionary<string, HBaseCell> data)
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

            HBaseCell cell;
            if (!data.TryGetValue(fieldName, out cell))
                throw Errors.DataItemFieldNotFound(fieldName);

            return cell.Value;
        }
    }
}
