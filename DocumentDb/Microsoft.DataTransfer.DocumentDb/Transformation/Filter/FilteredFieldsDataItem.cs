using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DataTransfer.DocumentDb.Transformation.Filter
{
    sealed class FilteredFieldsDataItem : DataItemWrapper
    {
        private ICollection<string> excludedFields;

        public FilteredFieldsDataItem(IDataItem dataItem, ICollection<string> excludedFields)
            : base(dataItem)
        {
            Guard.NotNull("excludedFields", excludedFields);
            this.excludedFields = excludedFields;
        }

        public override IEnumerable<string> GetFieldNames()
        {
            return base.GetFieldNames().Where(f => !excludedFields.Contains(f));
        }

        public override object GetValue(string fieldName)
        {
            // Note: Only top-level fields filtering is supported
            Guard.NotNull("fieldName", fieldName);

            if (excludedFields.Contains(fieldName))
                throw CommonErrors.DataItemFieldNotFound(fieldName);

            return base.GetValue(fieldName);
        }
    }
}
