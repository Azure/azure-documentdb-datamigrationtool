using Microsoft.DataTransfer.Extensibility;
using System;

namespace Microsoft.DataTransfer.DocumentDb.Transformation.Dates
{
    abstract class ConvertedDateTimeDataItemBase : TransformedDataItemBase
    {
        public ConvertedDateTimeDataItemBase(IDataItem dataItem)
            : base(dataItem) { }

        protected sealed override object TransformValue(object value)
        {
            if (value is DateTime)
                return ConvertDateTime((DateTime)value);

            return base.TransformValue(value);
        }

        protected abstract object ConvertDateTime(DateTime timeStamp);
        protected abstract override IDataItem TransformDataItem(IDataItem original);
    }
}
