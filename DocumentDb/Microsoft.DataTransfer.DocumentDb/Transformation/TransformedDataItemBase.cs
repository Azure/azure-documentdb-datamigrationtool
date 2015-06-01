using Microsoft.DataTransfer.Extensibility;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Transformation
{
    abstract class TransformedDataItemBase : DataItemWrapper
    {
        public TransformedDataItemBase(IDataItem dataItem)
            : base(dataItem) { }

        public sealed override object GetValue(string fieldName)
        {
            return TransformValue(base.GetValue(fieldName));
        }

        protected virtual object TransformValue(object value)
        {
            if (value is IDataItem)
                return TransformDataItem((IDataItem)value);

            if (value is IEnumerable && !(value is string))
            {
                var result = new List<object>();
                foreach (var item in (IEnumerable)value)
                    result.Add(TransformValue(item));
                return result;
            }

            return value;
        }

        protected abstract IDataItem TransformDataItem(IDataItem original);
    }
}
