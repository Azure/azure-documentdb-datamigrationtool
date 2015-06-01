using Microsoft.DataTransfer.Extensibility;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Transformation.Stringify
{
    sealed class StringifyFieldsTransformation : IDataItemTransformation
    {
        private readonly HashSet<string> fieldsToStringify;

        public StringifyFieldsTransformation(IEnumerable<string> fieldsToStringify)
        {
            this.fieldsToStringify = new HashSet<string>(fieldsToStringify);
        }

        public IDataItem Transform(IDataItem dataItem)
        {
            return new StringifiedFieldsDataItem(dataItem, fieldsToStringify);
        }
    }
}
