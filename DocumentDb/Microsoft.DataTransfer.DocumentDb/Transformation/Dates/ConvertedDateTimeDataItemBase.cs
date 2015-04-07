using Microsoft.DataTransfer.Extensibility;
using System;

namespace Microsoft.DataTransfer.DocumentDb.Transformation.Dates
{
    abstract class ConvertedDateTimeDataItemBase : DataItemWrapper
    {
        public ConvertedDateTimeDataItemBase(IDataItem dataItem)
            : base(dataItem) { }

        public override object GetValue(string fieldName)
        {
            var value = base.GetValue(fieldName);

            if (value is DateTime)
                return GetValue((DateTime)value);

            return value;
        }

        protected abstract object GetValue(DateTime timeStamp);
    }
}
