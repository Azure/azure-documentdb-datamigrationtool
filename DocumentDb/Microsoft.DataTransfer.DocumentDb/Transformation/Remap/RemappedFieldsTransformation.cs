using Microsoft.DataTransfer.Basics.Collections;
using Microsoft.DataTransfer.Extensibility;

namespace Microsoft.DataTransfer.DocumentDb.Transformation.Remap
{
    sealed class RemappedFieldsTransformation : IDataItemTransformation
    {
        private IReadOnlyMap<string, string> fieldsMapping;

        public RemappedFieldsTransformation(IReadOnlyMap<string, string> fieldsMapping)
        {
            this.fieldsMapping = fieldsMapping;
        }

        public IDataItem Transform(IDataItem dataItem)
        {
            return new RemappedFieldsDataItem(dataItem, fieldsMapping);
        }
    }
}
