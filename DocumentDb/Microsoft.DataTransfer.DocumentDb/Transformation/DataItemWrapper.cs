using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Transformation
{
    class DataItemWrapper : IDataItem
    {
        private IDataItem dataItem;

        public DataItemWrapper(IDataItem dataItem)
        {
            Guard.NotNull("dataItem", dataItem);
            this.dataItem = dataItem;
        }

        public virtual IEnumerable<string> GetFieldNames()
        {
            return dataItem.GetFieldNames();
        }

        public virtual object GetValue(string fieldName)
        {
            Guard.NotNull("fieldName", fieldName);
            return dataItem.GetValue(fieldName);
        }
    }
}
