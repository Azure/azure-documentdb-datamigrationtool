using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Transformation.Stringify
{
    sealed class StringifiedFieldsDataItem : DataItemWrapper
    {
        private readonly ICollection<string> fieldsToStringify;

        public StringifiedFieldsDataItem(IDataItem dataItem, ICollection<string> fieldsToStringify)
            : base(dataItem)
        {
            this.fieldsToStringify = fieldsToStringify;
        }

        public override object GetValue(string fieldName)
        {
            // Note: Only top-level fields stringification is supported
            Guard.NotNull("fieldName", fieldName);

            var value = base.GetValue(fieldName);
            return value != null && fieldsToStringify.Contains(fieldName) ? value.ToString() : value;
        }
    }
}
