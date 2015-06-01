using Microsoft.DataTransfer.Extensibility;
using System;

namespace Microsoft.DataTransfer.DocumentDb.Transformation.Dates
{
    sealed class EpochDateTimeDataItem : ConvertedDateTimeDataItemBase
    {
        public EpochDateTimeDataItem(IDataItem dataItem)
            : base(dataItem) { }

        protected override object ConvertDateTime(DateTime timeStamp)
        {
            return DateTimeConverter.ToEpoch(timeStamp);
        }

        protected override IDataItem TransformDataItem(IDataItem original)
        {
            return new EpochDateTimeDataItem(original);
        }
    }
}
