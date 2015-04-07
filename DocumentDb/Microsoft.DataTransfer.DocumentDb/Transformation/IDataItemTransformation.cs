using Microsoft.DataTransfer.Extensibility;

namespace Microsoft.DataTransfer.DocumentDb.Transformation
{
    interface IDataItemTransformation
    {
        IDataItem Transform(IDataItem dataItem);
    }
}
