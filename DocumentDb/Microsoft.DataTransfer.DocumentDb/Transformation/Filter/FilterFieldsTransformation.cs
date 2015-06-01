using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Transformation.Filter
{
    sealed class FilterFieldsTransformation : IDataItemTransformation
    {
        private HashSet<string> excludedFields;

        public FilterFieldsTransformation(IEnumerable<string> excludedFields)
        {
            Guard.NotNull("excludedFields", excludedFields);
            this.excludedFields = new HashSet<string>(excludedFields);
        }

        public IDataItem Transform(IDataItem dataItem)
        {
            Guard.NotNull("dataItem", dataItem);
            return new FilteredFieldsDataItem(dataItem, excludedFields);
        }
    }
}
