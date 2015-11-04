using Microsoft.DataTransfer.Basics;
using System;

namespace Microsoft.DataTransfer.JsonNet
{
    sealed class Errors : CommonErrors
    {
        private Errors() { }

        public static Exception GeometryTypeNotSupported(Type geometryType)
        {
            return new NotSupportedException(FormatMessage(Resources.GeometryTypeNotSupportedFormat, geometryType.Name));
        }
    }
}
