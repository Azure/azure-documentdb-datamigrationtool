using Microsoft.DataTransfer.Basics;
using System;

namespace Autofac
{
    sealed class Errors : CommonErrors
    {
        private Errors() { }

        public static Exception TypeNotFound(string typeName)
        {
            return new TypeLoadException(FormatMessage(Resources.TypeNotFoundFormat, typeName));
        }

        public static Exception NonGenericTypeForOpenGeneric(Type type)
        {
            return new TypeLoadException(FormatMessage(Resources.NonGenericTypeForOpenGenericFormat, type));
        }
    }
}
