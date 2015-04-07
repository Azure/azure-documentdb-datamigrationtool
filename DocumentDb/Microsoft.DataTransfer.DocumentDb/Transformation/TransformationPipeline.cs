using Microsoft.DataTransfer.Extensibility;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Transformation
{
    sealed class TransformationPipeline : IDataItemTransformation
    {
        private IEnumerable<IDataItemTransformation> transformations;

        public TransformationPipeline(IEnumerable<IDataItemTransformation> transformations)
        {
            this.transformations = transformations;
        }

        public IDataItem Transform(IDataItem dataItem)
        {
            foreach (var transformation in transformations)
                dataItem = transformation.Transform(dataItem);

            return dataItem;
        }
    }
}
