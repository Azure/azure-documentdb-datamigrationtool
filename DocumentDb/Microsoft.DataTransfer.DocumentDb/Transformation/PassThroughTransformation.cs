using Microsoft.DataTransfer.Extensibility;

namespace Microsoft.DataTransfer.DocumentDb.Transformation
{
    sealed class PassThroughTransformation : IDataItemTransformation
    {
        public static readonly IDataItemTransformation Instance = new PassThroughTransformation();

        public IDataItem Transform(IDataItem dataItem)
        {
            return dataItem;
        }
    }
}
