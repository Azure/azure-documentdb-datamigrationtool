using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Transformation.Dates
{
    sealed class StringAndEpochDateTimeDataItem : ConvertedDateTimeDataItemBase
    {
        private const string StringFieldName = "Value";
        private const string EpochFieldName = "Epoch";

        public StringAndEpochDateTimeDataItem(IDataItem dataItem)
            : base(dataItem) { }

        protected override object ConvertDateTime(DateTime timeStamp)
        {
            return new DictionaryDataItem(new Dictionary<string, object>
            {
                { StringFieldName, DateTimeConverter.ToString(timeStamp) },
                { EpochFieldName, DateTimeConverter.ToEpoch(timeStamp) }
            });
        }

        protected override IDataItem TransformDataItem(IDataItem original)
        {
            return new StringAndEpochDateTimeDataItem(original);
        }
    }
}
