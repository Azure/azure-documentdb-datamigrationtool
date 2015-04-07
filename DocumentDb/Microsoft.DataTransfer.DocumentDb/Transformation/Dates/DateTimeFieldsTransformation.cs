using Microsoft.DataTransfer.DocumentDb.Sink;
using Microsoft.DataTransfer.Extensibility;

namespace Microsoft.DataTransfer.DocumentDb.Transformation.Dates
{
    sealed class DateTimeFieldsTransformation : IDataItemTransformation
    {
        private DateTimeHandling handlingType;

        public DateTimeFieldsTransformation(DateTimeHandling handlingType)
        {
            this.handlingType = handlingType;
        }

        public IDataItem Transform(IDataItem dataItem)
        {
            switch (handlingType)
            {
                case DateTimeHandling.String:
                    return new StringDateTimeDataItem(dataItem);
                case DateTimeHandling.Epoch:
                    return new EpochDateTimeDataItem(dataItem);
                case DateTimeHandling.Both:
                    return new StringAndEpochDateTimeDataItem(dataItem);
                default:
                    return dataItem;
            }
        }
    }
}
