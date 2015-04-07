using Microsoft.DataTransfer.Basics;
using System;

namespace Microsoft.DataTransfer.WpfHost.Basics
{
    sealed class Errors : CommonErrors
    {
        private Errors() { }

        public static Exception InvalidTargetConvertionType(Type actualType, Type expectedType)
        {
            return new InvalidOperationException(FormatMessage(Resources.InvalidTargetConvertionType, actualType, expectedType));
        }

        public static Exception InvalidSourceConvertionType(Type actualType, Type expectedType)
        {
            return new InvalidOperationException(FormatMessage(Resources.InvalidSourceConvertionType, actualType, expectedType));
        }
    }
}
