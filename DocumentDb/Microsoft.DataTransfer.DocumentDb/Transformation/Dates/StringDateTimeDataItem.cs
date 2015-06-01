using Microsoft.DataTransfer.Extensibility;
using System;

namespace Microsoft.DataTransfer.DocumentDb.Transformation.Dates
{
    sealed class StringDateTimeDataItem : ConvertedDateTimeDataItemBase
    {
        public StringDateTimeDataItem(IDataItem dataItem)
            : base(dataItem) { }

        protected override object ConvertDateTime(DateTime timeStamp)
        {
            return DateTimeConverter.ToString(timeStamp);
        }

        protected override IDataItem TransformDataItem(IDataItem original)
        {
            return new StringDateTimeDataItem(original);
        }
    }
}
